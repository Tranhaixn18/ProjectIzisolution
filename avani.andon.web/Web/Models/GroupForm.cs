using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class GroupForm : tblUserGroup
    {
        public List<SelectListItem> Roles { get; set; }
        public string RoleName_VN { get; set; }
        public string RoleName_EN { get; set; }

        public bool iStatus { get; set; }
        public void cast(tblUserGroup n)
        {
            // this.Created_at = n.Created_at;
            this.CustomerId = n.CustomerId;
            this.Description = n.Description;
            this.Role = n.Role;
            tblUserRole role = new UserRoleDao().ViewDetail(this.Role);
            if (role != null)
            {
                this.RoleName_VN = role.Name_VN;
                this.RoleName_EN = role.Name_EN;
            }
            bool _status = false;
            if (n.Status != null)
            {
                _status = (bool)n.Status;
            }
            this.Status = _status;
            this.iStatus = _status;

            this.Name = n.Name;
            this.Id = n.Id;
        }
        public tblUserGroup cast()
        {
            return new tblUserGroup()
            {
                Id = this.Id,
                Name = this.Name,
                Status = iStatus,
                Description = this.Description,
                Role = this.Role,
                CustomerId = this.CustomerId
            };
        }
    }
}