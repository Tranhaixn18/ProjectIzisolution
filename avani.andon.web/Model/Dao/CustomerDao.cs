using Common;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class CustomerDao
    {
        AvaniDataContext db = null;
        public CustomerDao()
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

        public long Insert(tblCustomer entity)
        {
            try
            {
                db.tblCustomers.InsertOnSubmit(entity);
                db.SubmitChanges();
                // create group id
                int ID = entity.Id;
                int isAdmin = 1;
                tblUserGroup g = new tblUserGroup();
                g.Name = "admin_" + entity.Name;
                g.Status = true;
                g.CustomerId = ID;
                g.Description = "Create new by customer";
                g.Role = Common.GlobalConstants.AV_ADMIN_GROUP;
                new UserGroupDao().Insert(g);
                // create user
                tblUser u = new tblUser();
                u.GroupId = g.Id;
                u.Email = entity.Email;
                u.Status = true;
                UserDao uDao = new UserDao();
                //  string genpass = Encryptor.CreateRandomPassword(6);
                string genpass = "123";
                string passHash = Encryptor.MD5Hash(genpass);
                u.Password = passHash;
                u.UserName = "admin_" + entity.Name;
                uDao.Insert(u);
                //sendmail
                // MailHelper send = new MailHelper();
                // send.SendMail(entity.Email, "Application avEMS", "Account login to system \r\n Username:"+u.UserName+ " \r\n  Password:" + genpass);
                // create menu
                // List<tblMenu> lstMenu =  new MenuDao().listMenuDefault();
                //  foreach(tblMenu m in lstMenu)
                //  {
                //      new PermissionDao().InsertMenu(m);
                //  }

            }
            catch { }
            return entity.Id;
        }

        public bool Update(tblCustomer entity)
        {
            try
            {
                var tblCustomer = db.tblCustomers.SingleOrDefault(x => x.Id == entity.Id);
                tblCustomer.Description = entity.Description;
                tblCustomer.Address = entity.Address;
                tblCustomer.Email = entity.Email;
                tblCustomer.Status = entity.Status;
                tblCustomer.Logo = entity.Logo;
                tblCustomer.Phone = entity.Phone;
                tblCustomer.FullName = entity.FullName;
                tblCustomer.Name = entity.Name;
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblCustomer> listAll()
        {
            int CustomerID = Common.GlobalConstants.getCustomer();
            if (CustomerID == 0)
            {
                return db.tblCustomers.ToList();
            }
            else
            {
                return db.tblCustomers.Where(x => x.Id == CustomerID).ToList();
            }
        }

        public tblCustomer ViewDetail(int id)
        {
            try
            {
                return db.tblCustomers.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var Customer = db.tblCustomers.SingleOrDefault(x => x.Id == id);
                db.tblCustomers.DeleteOnSubmit(Customer);
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
