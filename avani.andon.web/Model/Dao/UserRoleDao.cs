using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class UserRoleDao
    {
        AvaniDataContext db = null;
        public UserRoleDao()
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

        public long Insert(tblUserRole entity)
        {
            try
            {
                db.tblUserRoles.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch { }
            return entity.Id;
        }

        public bool Update(tblUserRole entity)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblUserRole> listAll()
        {
            return db.tblUserRoles.ToList();
        }
        public tblUserRole ViewDetail(int id)
        {
            try
            {
                return db.tblUserRoles.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public tblUserRole ViewDetail(string code)
        {
            try
            {
                return db.tblUserRoles.SingleOrDefault(x => x.Code == code);
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                var Group = db.tblUserRoles.SingleOrDefault(x => x.Id == id);
                db.tblUserRoles.DeleteOnSubmit(Group);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
