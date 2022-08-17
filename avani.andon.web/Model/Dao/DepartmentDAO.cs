using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DepartmentDAO
    {
        AvaniDataContext db = null;
        public DepartmentDAO()
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

        public List<tblDepartment> GetAll()
        {
            return db.tblDepartments.Select(tbl => tbl).ToList();
        }
        public long insert(tblDepartment entity)
        {
            if (entity.Code != null && entity.Name != null && entity.Description != null)
            {
                var empCode = db.tblDepartments.FirstOrDefault(x => x.Code == entity.Code);
                if (empCode != null)
                {
                    return 0;
                }
                var empName = db.tblDepartments.FirstOrDefault(x => x.Name == entity.Name);
                if (empName != null)
                {
                    return -1;
                }
                var empDes = db.tblDepartments.FirstOrDefault(x => x.Description == entity.Description);
                if (empDes != null)
                {
                    return -2;
                }

                else
                {
                    db.tblDepartments.InsertOnSubmit(entity);
                    db.SubmitChanges();
                }

                return entity.Id;
            }
            return -3;

        }

        public int Update(tblDepartment entity)
        {
            try
            {


                //if (db.tblDepartments.Any(x => x.Code == entity.Code) && db.tblDepartments.All(x => x.Id != entity.Id))
                //{

                //    return 0;
                //}
                //else if (db.tblDepartments.Any(x => x.Name == entity.Name) && db.tblDepartments.All(x => x.Id != entity.Id))
                //{

                //    return -1;

                //}
                //else if (db.tblDepartments.All(x => x.Description == entity.Description) && db.tblDepartments.Any(x => x.Id != entity.Id))
                //{

                //    return -2;
                //}
                //else
                //{
                List<tblDepartment> list = db.tblDepartments.Where(x => x.Id != entity.Id).ToList();
                foreach (var item in list)
                {
                    if (item.Code == entity.Code)
                    {
                        return 0;
                    }
                    /*if (item.Name == entity.Name)
                    {
                        return -1;
                    }
                    if (item.Description == entity.Description)
                    {
                        return -2;
                    }*/

                }
                var department = db.tblDepartments.SingleOrDefault(x => x.Id == entity.Id);
                department.Code = entity.Code;
                department.Description = entity.Description;
                department.Name = entity.Name;
                db.SubmitChanges();
                return 1;

                //}

            }
            catch (Exception ex)
            {
                //logging
                return 0;
            }

        }
        public tblDepartment ViewDetail(long id)
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
        public bool Delete(int id)
        {

            List<tblEmployee> list = db.tblEmployees.Where(tbl => tbl.DeptId == id).ToList();
            if (list.Count == 0)
            {
                var depart = db.tblDepartments.SingleOrDefault(x => x.Id == id);
                db.tblDepartments.DeleteOnSubmit(depart);
                db.SubmitChanges();
                return true;
            }
            return false;
        }
        public List<tblEmployee> ListEmployeeByDepartmentID(int id)
        {
            try
            {
                return db.tblEmployees.Where(tbl => tbl.DeptId == id).ToList();
            }
            catch { return null; }
        }
    }
}
