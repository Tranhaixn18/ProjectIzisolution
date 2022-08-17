using Common;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        AvaniDataContext db = null;
        public MenuDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            }
            else
            {
                db = new AvaniDataContext();
            }
        }
        public List<tblMenu> listMenuDefault()
        {
            return db.tblMenus.Where(x => x.CustomerId == 0 && x.IsAdmin == false).ToList();
        }
        public List<tblMenu> GetMenuByPermission(int Role, List<tblUserPermission> listPer, tblUserGroup g)
        {
            List<tblMenu> listMenu = new List<tblMenu>().OrderBy(x => x.nOrder).ToList();
            if (Role == GlobalConstants.ROLE_SUPPER_ADMIN)
            {

            }
            else
            {
                if (Role == GlobalConstants.ROLE_ADMIN)
                {
                    listMenu = listMenu.Where(x => x.IsSuperAdmin == false && x.IsActive == true && (x.CustomerId == 0 || x.CustomerId == g.CustomerId)).OrderBy(x => x.nOrder).ToList();
                }
                else
                {
                    listMenu = listMenu.Where(x => x.IsAdmin == false && x.IsSuperAdmin == false && x.IsActive == true && (x.CustomerId == 0 || x.CustomerId == g.CustomerId)).OrderBy(x => x.nOrder).ToList();
                    List<tblMenu> returnMenu = new List<tblMenu>();
                    foreach (tblMenu item in listMenu)
                    {
                        foreach (tblUserPermission data in listPer)
                        {
                            if (data.ObjectType == GlobalConstants.MENU_OBJECT_TYPE && item.Id == data.ObjectId && g.Id == data.GroupId && data.View == true)
                            {
                                returnMenu.Add(item);
                                break;
                            }
                        }
                    }
                    return returnMenu;
                }
            }
            return listMenu;
        }
    }
}
