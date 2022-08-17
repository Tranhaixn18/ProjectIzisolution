using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class EmployeeDao
    {
        AvaniDataContext db = null;
        public EmployeeDao()
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
        public List<tblEmployee> GetAll()
        {
            return db.tblEmployees.ToList();
        }
        public int Insert(tblEmployee entity)
        {
            var empl = db.tblEmployees.FirstOrDefault(x => x.CardId == entity.CardId);
            //if (empl != null && empl.Active == true)
            //{
            //    return 0;
            //}
            var emplCode = db.tblEmployees.FirstOrDefault(x => x.Code == entity.Code);
            if (emplCode != null)
            {
                return -1;
            }
            var emplPhone = db.tblEmployees.FirstOrDefault(x => x.Phone == entity.Phone);
            if (emplPhone != null)
            {
                return -2;
            }
            else
            {
                db.tblEmployees.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            return entity.Id;
        }
        public tblEmployee ViewDetail(int id)
        {
            try
            {
                return db.tblEmployees.SingleOrDefault(x => x.Id == id);
            }
            catch { return null; }
        }
        public int Update(tblEmployee request)
        {
            try
            {
                if (db.tblEmployees.Any(x => x.CardId == request.CardId &&  x.Id != request.Id))
                {
                    return 0;
                }
                else if (db.tblEmployees.Any(x => x.Phone == request.Phone && x.Id != request.Id))
                {
                    return -1;
                }
                else
                {
                    var empl = db.tblEmployees.SingleOrDefault(x => x.Id == request.Id);
                    if (empl != null)
                    {
                        empl.Code = request.Code;
                        empl.Name = request.Name;
                        empl.Description = request.Description;
                        empl.Email = request.Email;
                        empl.Phone = request.Phone;
                        empl.CardId = request.CardId;
                        empl.DeptId = request.DeptId;
                        //empl.Active = request.Active;
                    }
                    db.SubmitChanges();
                    return 1;
                }

            }
            catch (Exception)
            {
                return -2;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var empl = db.tblEmployees.SingleOrDefault(x => x.Id == id);
                db.tblEmployees.DeleteOnSubmit(empl);
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
