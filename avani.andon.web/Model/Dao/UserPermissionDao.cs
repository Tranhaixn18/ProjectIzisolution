using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserPermissionDao
    {
        AvaniDataContext db = null;
        public UserPermissionDao()
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
        public List<tblUserPermission> FindByGroupId(int groupId)
        {
            return db.tblUserPermissions.Where(x => x.GroupId == groupId).ToList();
        }
        public void insertGroup(tblUserGroup g)
        {
            bool _view = false, _update = false;

            if (g.Role != null)
            {
                if (g.Role == "ADMIN")
                {
                    _update = true;
                    _view = true;
                }
                if (g.Role == "MANAGER")
                {
                    _update = false;
                    _view = true;
                }
            }

            List<tblUserPermission> lstPermission = new List<tblUserPermission>();
            int _customerId = (int)g.CustomerId;
            // Line
            List<tblLine> lstLine = new LineDao().ListAllByCustomer(_customerId);
            foreach (tblLine z in lstLine)
            {
                tblUserPermission p = new tblUserPermission();
                p.GroupId = g.Id;
                p.ObjectType = Common.GlobalConstants.LINE_OBJECT_TYPE;
                p.ObjectId = z.Id;
                p.Update = _update;
                p.View = _view;
                lstPermission.Add(p);
            }
            // device
            /*List<tblNode> lstNode = new NodeDao().ListAllByCustomerId((int)g.CustomerId);
            foreach (tblNode n in lstNode)
            {
                tblUserPermission p = new tblUserPermission();
                p.GroupId = g.Id;
                p.ObjectType = Common.GlobalConstants.NODE_OBJECT_TYPE;
                p.ObjectId = n.Id;
                p.Update = _update;
                p.View = _view;
                lstPermission.Add(p);
            }*/
            // menu
            List<tblMenu> lstMenu = new MenuDao().listMenuDefault();
            foreach (tblMenu m in lstMenu)
            {
                tblUserPermission p = new tblUserPermission();
                p.GroupId = g.Id;
                p.ObjectType = Common.GlobalConstants.MENU_OBJECT_TYPE;
                p.ObjectId = m.Id;
                p.Update = false;
                if ((bool)m.IsAdmin || (bool)m.IsSuperAdmin)
                {
                    p.View = false;
                }
                else
                {
                    p.View = true;
                }

                lstPermission.Add(p);
            }
            insertAll(lstPermission);
        }
        public List<tblUserPermission> findByGroupID(int GroupID)
        {
            return db.tblUserPermissions.Where(x => x.GroupId == GroupID).ToList();
        }
        public bool Update(tblUserPermission entity)
        {
            try
            {
                var tblPermission = db.tblUserPermissions.SingleOrDefault(x => x.Id == entity.Id);
                tblPermission.View = entity.View;
                tblPermission.Update = entity.Update;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public List<tblUserPermission> findByObject(int ObjectId, int ObjectType)
        {
            return db.tblUserPermissions.Where(x => x.ObjectId == ObjectId && x.ObjectType == ObjectType).ToList();
        }
        
        public void insertAll(List<tblUserPermission> lstPermission)
        {
            db.tblUserPermissions.InsertAllOnSubmit(lstPermission);
            db.SubmitChanges();
        }

    }
}
