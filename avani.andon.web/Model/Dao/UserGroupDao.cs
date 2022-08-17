using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserGroupDao
    {
        AvaniDataContext db = null;
        public UserGroupDao()
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
        public bool Update(tblUserGroup entity)
        {
            try
            {
                var tblGroup = db.tblUserGroups.SingleOrDefault(x => x.Id == entity.Id);
                tblGroup.Role = entity.Role;
                tblGroup.Status = entity.Status;
                tblGroup.CustomerId = entity.CustomerId;
                tblGroup.Description = entity.Description;
                tblGroup.Name = entity.Name;

                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public bool Delete(long id)
        {
            try
            {
                var Group = db.tblUserGroups.SingleOrDefault(x => x.Id == id);
                db.tblUserGroups.DeleteOnSubmit(Group);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public tblUserGroup ViewDetail(int Id)
        {
            try
            {
                return db.tblUserGroups.SingleOrDefault(x => x.Id == Id);
            }
            catch
            {
                return null;
            }
        }
        public List<tblUserGroup> GetAll()
        {
            return db.tblUserGroups.ToList();
        }
        public List<tblUserGroup> GetAllByCustomerId(int customerId)
        {
            return db.tblUserGroups.Where(x => x.CustomerId == customerId).ToList();
        }
        public List<tblUserGroup> listAll()
        {
            return db.tblUserGroups.ToList();
        }
        public List<tblUserGroup> listAll(int customerID)
        {
            return db.tblUserGroups.Where(x => x.CustomerId == customerID).ToList();
        }

        public long Insert(tblUserGroup entity)
        {

            db.tblUserGroups.InsertOnSubmit(entity);
            db.SubmitChanges();
            new UserPermissionDao().insertGroup(entity);

            return entity.Id;
        }

    }
}
