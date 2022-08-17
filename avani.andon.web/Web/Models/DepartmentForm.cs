using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class DepartmentForm:tblDepartment
    {
        
        public void Cast(tblDepartment n)
        {
            // this.Created_at = n.Created_at;
            this.Id = n.Id;
            this.Description = n.Description;
            this.Name = n.Name;
            this.Code = n.Code;
            //this.RF_ID = n.RF_ID;
            //this.RF_Code = n.RF_Code;
        }
        public tblDepartment Cast()
        {
            return new tblDepartment()
            {
                Id = this.Id,
                Name = this.Name,
                Code = this.Code,
                Description = this.Description,
                //RF_ID = this.RF_ID,
                //RF_Code=this.RF_Code,
            };
        }
        public long create()
        {
            DepartmentDAO nodedao = new DepartmentDAO();
            return nodedao.insert(Cast());
        }

        public void Update()
        {
            DepartmentDAO nodeDao = new DepartmentDAO();
            nodeDao.Update(Cast());
        }

    }
}