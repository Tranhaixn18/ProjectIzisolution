using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class CustomerForm : tblCustomer
    {
        public string StatusClass { get; set; }
        public string StatusName { get; set; }
        public bool iStatus { get; set; }
        public HttpPostedFileBase LogoUpload { get; set; }
        public void cast(tblCustomer n)
        {
            this.FullName = n.FullName;
            this.Phone = n.Phone;
            this.Email = n.Email;
            this.Address = n.Address;
            this.Logo = n.Logo;
            this.Description = n.Description;
            this.Status = n.Status;
            if (n.Status != null && n.Status == true)
            {
                StatusClass = "label-success";
                StatusName = Resources.Language.Active;
                iStatus = Convert.ToBoolean(n.Status);
            }
            else
            {
                iStatus = false;
                StatusClass = "label-danger";
                StatusName = "Status_InActive";
            }
            this.Name = n.Name;
            this.Id = n.Id;
        }
        public tblCustomer cast()
        {
            return new tblCustomer()
            {
                Id = this.Id,
                Name = this.Name,
                Status = this.iStatus,
                Description = this.Description,
                Address = this.Address,
                Email = this.Email,
                Phone = this.Phone,
                Logo = this.Logo,
                FullName = this.FullName

            };
        }
    }
}