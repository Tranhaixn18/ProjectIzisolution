using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class EmployeeForm :tblEmployee
    {
        public string DepartName { get; set; }
        public List<SelectListItem> Depts { get; set; }
        public string ActiveName { get; set; }
        public void Cast(tblEmployee empl)
        {
            this.Id = empl.Id;
            this.Name = empl.Name;
            this.Title = empl.Title;
            this.Email = empl.Email;
            this.Phone = empl.Phone;
            this.Description = empl.Description;
            this.CardId = empl.CardId;
            //this.RFCode = empl.RFCode;
            //this.RFId = empl.RFId;
            //this.Active = empl.Active;
            this.Code = empl.Code;
            this.CardId = empl.CardId;
            tblDepartment d = new DepartDao().ViewDetail(Convert.ToInt32(empl.DeptId));
            if (d != null)
            {
                this.DepartName = d.Name;
            }
            //if (empl.Active == true)
            //{
            //    this.ActiveName = "Đang làm";
            //}
            //else
            //{
            //    this.ActiveName = "Thôi việc";
            //}

        }
        public tblEmployee GetBaseEmpl(EmployeeForm entity)
        {
            tblEmployee empl = new tblEmployee();
            empl.Id = entity.Id;
            empl.Code = entity.Code;
            empl.Name = entity.Name;
            empl.Title = entity.Title;
            empl.Email = entity.Email;
            empl.Phone = entity.Phone;
            empl.Description = entity.Description;
            empl.CardId = entity.CardId;
            empl.DeptId = entity.DeptId;
           // empl.Active = entity.Active;
            return empl;
        }
        public EmployeeForm GetEmplForm(tblEmployee entity)
        {
            EmployeeForm empl = new EmployeeForm();
            empl.Id = entity.Id;
            empl.Code = entity.Code;
            empl.Name = entity.Name;
            empl.Title = entity.Title;
            empl.Description = entity.Description;

            int deptId = entity.DeptId == null ? 0 : Convert.ToInt32(entity.DeptId);
            empl.DeptId = deptId;
            empl.CardId = entity.CardId;
            //empl.RFId = entity.RFId;
            //empl.RFCode = entity.RFCode;
            //empl.Active = entity.Active;
            empl.Email = entity.Email;
            empl.Phone = entity.Phone;

            tblDepartment dept = new DepartDao().ViewDetail(deptId);
            if (dept != null)
            {
                empl.DepartName = dept.Name;
            }
            return empl;
        }
    }
}