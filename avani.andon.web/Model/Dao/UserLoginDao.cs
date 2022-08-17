using Model.DataModel;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Model.Dao
{
    public class UserLoginDao
    {

        AvaniDataContext db = null;

        public UserLoginDao()
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
        

        public int GetMaxId()
        {
            var result = db.tblUserLogins.OrderByDescending(x => x.Id).FirstOrDefault();
            if (result is null)
            {
                return 0;
            }
            return result.Id;

        }
        public int Create(tblUserLogin userLogin)
        {
            try
            {
                userLogin.Id = this.GetMaxId() + 1;
                db.tblUserLogins.InsertOnSubmit(userLogin);
                db.SubmitChanges();
            }
            catch (Exception)
            {
            }
            return userLogin.Id;

        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.tblUserLogins.SingleOrDefault(x => x.Id == id);
                db.tblUserLogins.DeleteOnSubmit(user);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool ChangeStatus(int user_id)
        {
            try
            {
                var user = db.tblUserLogins.OrderByDescending(x => x.Id).FirstOrDefault(x => x.User_Id == user_id);
                if (user is null)
                {
                    return false;
                }
                else
                {
                    user.State = false;
                    db.SubmitChanges();
                }
               
                return true;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public bool checkToken(string token)
        {
            try
            {
                var today = DateTime.Now;
                var user = db.tblUserLogins.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Token == token & x.State == true & x.Expire_Date >= today);
                if (user is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception er)
            {
                return false;
            }
        }
        public ArrayList getDepartments()
        {
            ArrayList result = new ArrayList();
            try
            {
                //var employees = db.tblEmployees.Where(x => x.Active == true).ToList();
                var query = from p in db.tblDepartments
                             select new
                             {
                                 Id = p.Id,
                                 Code = p.Code,
                                 Name = p.Name

                             };
                foreach (var emp in query)
                {
                    result.Add(new
                    {
                        Id = emp.Id,
                        Code = emp.Code,
                        Name = emp.Name
                    });
                }
                return result;
            }
            catch(Exception er)
            {
                return result;
            }
        }

        public ArrayList getEmployees()
        {
            ArrayList result = new ArrayList();
            try
            {
                //var employees = db.tblEmployees.Where(x => x.Active == true).ToList();
                var query = from p in db.tblEmployees
                            join c in db.tblDepartments on p.DeptId equals c.Id
                             select new
                             {
                                 Id = p.Id,
                                 Code = p.Code,
                                 Name = p.Name,
                                 Department = c.Name,
                                 Phone = p.Phone,
                                 Card = p.CardId,
                                 Email = p.Email,
                                 Description = p.Description,
                               //  Active = p.Active

                             };
                foreach (var emp in query)
                {
                    result.Add(new
                    {
                        Id = emp.Id,
                        Code = emp.Code,
                        Name = emp.Name,
                        Depart = emp.Department,
                        Phone = emp.Phone,
                        Card = emp.Card,
                        Email= emp.Email,
                        Description = emp.Description,
                       // Active = emp.Active,
                    });
                }
                return result;
            }
            catch(Exception er)
            {
                return result;
            }
        }



    }
}
