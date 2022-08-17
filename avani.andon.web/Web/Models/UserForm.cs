using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class UserForm : tblUser
    {
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Langs { get; set; }
        public List<SelectListItem> Lines { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordReNew { get; set; }
        public string LineName { get; set; }
        public string GroupName { get; set; }
        public string GroupClass { get; set; }
        public HttpPostedFileBase AvatarUpload { get; set; }
        public tblUser GetBaseUser(UserForm entity)
        {
            tblUser user = new tblUser();
            user.ID = entity.ID;
            user.UserName = entity.UserName;
            user.Password = entity.Password;
            user.Role = entity.Role;
            user.FullName = entity.FullName;
            user.Email = entity.Email;
            user.Phone = entity.Phone;
            user.Avatar = entity.Avatar;
            user.CreatedDate = entity.CreatedDate;
            user.Status = entity.Status;
            user.Lang = entity.Lang;
            user.LineId = entity.LineId;

            return user;
        }

        public UserForm GetUserForm (tblUser entity)
        {
            UserForm user = new UserForm();
            user.ID = entity.ID;
            user.UserName = entity.UserName;
            user.Password = entity.Password;
            user.Role = entity.Role;
            user.FullName = entity.FullName;
            user.Email = entity.Email;
            user.Phone = entity.Phone;
            user.Avatar = entity.Avatar;
            user.CreatedDate = entity.CreatedDate;
            user.Status = entity.Status;
            user.Lang = entity.Lang;
            int lineId = entity.LineId==null?0:Convert.ToInt32(entity.LineId);
            user.LineId = lineId;
            tblLine ll = new Model.Dao.LineDao().ViewDetail(lineId);
            if (ll != null)
            {
                user.LineName = ll.Name;
            }
            if (entity.GroupId != null)
            {
                tblUserGroup g = new UserGroupDao().ViewDetail(Convert.ToInt32(entity.GroupId));
                if (g != null)
                {
                    user.GroupName = g.Name;
                }
            }
            return user;
        }
    }
}