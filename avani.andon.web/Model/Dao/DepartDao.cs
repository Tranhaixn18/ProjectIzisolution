using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DepartDao
    {
        AvaniDataContext db = null;
        public DepartDao()
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
        public tblDepartment ViewDetail(int id)
        {
            try
            {
                return db.tblDepartments.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public List<tblDepartment> listAll()
        {
            return db.tblDepartments.ToList();
        }
    }
}
