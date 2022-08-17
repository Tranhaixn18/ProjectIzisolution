using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.DataModel;
using avSVAW.Models;
using avSVAW.App_Start;
using System.Configuration;
using System.Drawing;

namespace avSVAW.Controllers
{
    [SessionTimeout]
    public class UserController : BaseController
    {
        public static bool Is_Sumiden = Convert.ToBoolean(ConfigurationManager.AppSettings["Is_Sumiden"].ToString());
        public ActionResult Index()
        {
            var group = (tblUserGroup)Session[GlobalConstants.GROUP_SESSION];
            var role = Convert.ToInt32(Session[GlobalConstants.ROLE_SESSION]);
            var dao = new UserDao();
            List<tblUser> lstUser = new List<tblUser>();
            if (role == GlobalConstants.ROLE_ADMIN)
            {
                lstUser = dao.ListAll();
            }
            else
            {
                lstUser = dao.ListAll(group);
            }
            Random rnd = new Random();
            var model = new List<UserForm>();
            List<int> groupid = new List<int>();
            List<string> groupColor = new List<string>();
            foreach (tblUser u in lstUser)
            {
                string color = "#000";
                if (u.GroupId != null)
                {
                    int i = groupid.FindIndex(x => x == u.GroupId);
                    if (i >= 0)
                    {
                        color = groupColor[i];
                    }
                    else
                    {
                        Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                        groupColor.Add(HexConverter(randomColor));
                        groupid.Add(Convert.ToInt32(u.GroupId));
                        color = HexConverter(randomColor);
                    }
                }
                UserForm f = new UserForm().GetUserForm(u);
                f.GroupClass = color;

                model.Add(f);
            }

            return View(model);
        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        [HttpGet]
        public ActionResult Create()
        {
            Models.UserForm userModel = EditOrCreate(null);
            return View(userModel);
        }
        [HttpPost]
        public ActionResult Create(UserForm userForm)
        {
            if (ModelState.IsValid)
            {
                tblUser userObj = new UserForm().GetBaseUser(userForm);
                //validate
                if (userObj != null)
                {
                    var userExist = new UserDao().GetBytblUserName(userObj.UserName);
                    if (userExist != null)
                    {
                        //da ton tai user nay
                        ModelState.AddModelError("", "Đã tồn tại User này");
                        Models.UserForm userModel = EditOrCreate(null);
                        userModel.UserName = userExist.UserName;
                        return View(userModel);
                    }
                }
                //string server_path = Server.MapPath(GlobalConstants.AVATAR_LOCATION);

                //userObj.Avatar = server_path + "/" + userObj.Role + ".png";                
                //userObj.Avatar = server_path + "/user.png";

                var dao = new UserDao();
                var encryptedMd5Pas = Encryptor.MD5Hash(userObj.Password);
                userObj.Password = encryptedMd5Pas;

                long id = dao.Insert(userObj);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View();
        }

        public Models.UserForm EditOrCreate(tblUser UserObj = null)
        {
            Models.UserForm userModel = new UserForm();

            if (UserObj != null)
            {
                userModel = new UserForm().GetUserForm(UserObj);
            }
            else
            {
                userModel.Status = true; //default cho form create la kich hoat trang thai
            }


            userModel.CreatedDate = DateTime.Now;

            userModel.Langs = new List<SelectListItem>();

            userModel.Langs.Add(new SelectListItem() { Value = GlobalConstants.LANG_VI, Text = GlobalConstants.LANG_VI_Text });
            userModel.Langs.Add(new SelectListItem() { Value = GlobalConstants.LANG_EN, Text = GlobalConstants.LANG_EN_Text });

            userModel.Roles = new List<SelectListItem>();


            //   userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_PRODUCTION_GROUP, Text = GlobalConstants.ARIS_PRODUCTION_GROUP });
            //  userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_QUALITY_GROUP, Text = GlobalConstants.ARIS_QUALITY_GROUP });
            //  userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_MAINTENANCE_GROUP, Text = GlobalConstants.ARIS_MAINTENANCE_GROUP });
            // userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_LOGISTICS_GROUP, Text = GlobalConstants.ARIS_LOGISTICS_GROUP });
            //  userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_MANAGER_GROUP, Text = GlobalConstants.ARIS_MANAGER_GROUP });
            userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.AV_ADMIN_GROUP, Text = GlobalConstants.AV_ADMIN_GROUP });
            userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.ARIS_LINE_LEADER_GROUP, Text = GlobalConstants.ARIS_LINE_LEADER_GROUP });
            userModel.Roles.Add(new SelectListItem() { Value = GlobalConstants.AV_STAFF_GROUP, Text = GlobalConstants.AV_STAFF_GROUP });

            List<tblLine> lstLines = new LineDao().listAll();
            userModel.Lines = new List<SelectListItem>();
            userModel.Lines.Add(new SelectListItem() { Value = "", Text = Resources.Language.All });
            for (int i = 0; i < lstLines.Count; i++)
            {
                userModel.Lines.Add(new SelectListItem() { Value = lstLines[i].Id.ToString(), Text = lstLines[i].Name });
            }

            //userModel.RoleListItem.Add(new SelectListItem() { Value = GlobalConstants.MT_USER_GROUP, Text = "Người dùng" });


            return userModel;
        }


        public ActionResult SelfEdit(int id)
        {
            var userObj = new UserDao().ViewDetail(id);




            ViewBag.lang = userObj.Lang;
            //ViewBag.DepartmentName = new DepartmentDao().GetDepartmentNameById(userObj.DepartmentId);
            //string RoleName = "";
            //switch (userObj.Role)
            //{
            //    case "MANAGER":
            //        RoleName = "Giám đốc";
            //        break;
            //    //case "ACCOUNTANT":
            //    //    RoleName = "Kế toán";
            //    //    break;
            //    case "STAFF":
            //        RoleName = "Giao nhận";
            //        break;
            //    case "ADMIN":
            //        RoleName = "Quản trị";
            //        break;
            //}
            ViewBag.RoleName = userObj.Role;

            return View(userObj);
        }

        [HttpPost]
        public ActionResult SelfEdit(tblUser userObj)
        {
            if (ModelState.IsValid)
            {

                var dao = new UserDao();
                if (!string.IsNullOrEmpty(userObj.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(userObj.Password);
                    userObj.Password = encryptedMd5Pas;
                }
                var result = dao.Update(userObj, true);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View(userObj);
        }


        public ActionResult Edit(int id)
        {
            var userObj = new UserDao().ViewDetail(id);
            Models.UserForm userModel = EditOrCreate(userObj);


            return View(userModel);
        }


        [HttpPost]
        public ActionResult Edit(UserForm userForm, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                tblUser userObj = new UserForm().GetBaseUser(userForm);


                var dao = new UserDao();
                if (!string.IsNullOrEmpty(userObj.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(userObj.Password);
                    userObj.Password = encryptedMd5Pas;
                }
                var result = dao.Update(userObj);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View(userForm);
        }


        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);

            return RedirectToAction("Index");
        }

        public Models.UserForm EditOrCreate1(tblUser UserObj = null)
        {
            Models.UserForm userModels = new UserForm();
            if (UserObj != null)
            {
                userModels = new UserForm().GetUserForm(UserObj);
            }
            else
            {
                userModels.Status = true;
            }
            userModels.Groups = new List<SelectListItem>();
            var group = (tblUserGroup)Session[GlobalConstants.GROUP_SESSION];
            //var role = Convert.ToInt32(Session[GlobalConstants.ROLE_SESSION]);
            List<tblUserGroup> listGroup = new List<tblUserGroup>();
            // //if (role == GlobalConstants.ROLE_SUPPER_ADMIN)
            // {
            //     listGroup = new UserGroupDao().GetAll();
            // }
            // else
            // {
            //   if (group.CustomerId != null)
            //   {
            //        listGroup = new UserGroupDao().GetAllByCustomerId(Convert.ToInt32(group.CustomerId));
            //    }
            // }
            foreach (tblUserGroup item in listGroup)
            {
                userModels.Groups.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return userModels;
        }
        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            var user = new UserDao().ViewDetail(id);
            Models.UserForm userModel = EditOrCreate1(user);
            userModel.Password = "";
            return View(userModel);
        }
        [HttpPost]
        public ActionResult ChangePassword(UserForm userForm)
        {
            tblUser userObj = new UserForm().GetBaseUser(userForm);
            var userOld = new UserDao().ViewDetail(userForm.ID);
            if (!string.IsNullOrEmpty(userObj.Password) && !string.IsNullOrEmpty(userForm.PasswordNew) && !string.IsNullOrEmpty(userForm.PasswordReNew))
            {
                if (userForm.PasswordNew == userForm.PasswordReNew)
                {
                    var md5Pass = Encryptor.MD5Hash(userObj.Password);
                    if (userOld.Password == md5Pass)
                    {
                        var userDao = new UserDao();
                        userObj.Password = Encryptor.MD5Hash(userForm.PasswordNew);
                        var result = userDao.UpdatePassword(userObj);
                        //SetAlert(Resources.Language.AlertMonitor,"Đổi mật khẩu thành công");
                        ModelState.AddModelError("", "Thay đổi mật khẩu thành công");
                        return View(userForm);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu không chính xác");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nhập lại mật khẩu không chính xác");
                }
            }
            else
            {
                ModelState.AddModelError("", "Đổi mật khẩu không thành công");
            }
            return View(userForm);
        }

        [HttpGet]
        public ActionResult ChangeInfor(int id)
        {
            var user = new UserDao().ViewDetail(id);
            Models.UserForm userForm = new UserForm();
            if (user != null)
            {
                userForm = new UserForm().GetUserForm(user);

            }
            return View(userForm);
        }
        [HttpPost]
        public ActionResult ChangeInfor(UserForm request)
        {
            if (ModelState.IsValid)
            {
                var userDao = new UserDao();
                tblUser user = new UserDao().ViewDetail(request.ID);
                if (!string.IsNullOrEmpty(user.FullName))
                {
                    user.FullName = request.FullName;

                }
                if (!string.IsNullOrEmpty(user.Email))
                {
                    user.Email = request.Email;
                }
                if (!string.IsNullOrEmpty(user.Phone))
                {
                    user.Phone = request.Phone;
                }
                var file = request.AvatarUpload;
                string random = Encryptor.CreateRandomPassword(5);
                string strDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (file != null)
                {
                    string _fileName = random + "_" + strDate + Path.GetExtension(file.FileName);
                    string _path = Path.Combine(Directory.GetCurrentDirectory(),Server.MapPath("~/Uploads/Avatar"), _fileName);
                    file.SaveAs(_path);

                    user.Avatar = _fileName;
                }
                var result = userDao.Update(user, true);
                if (result)
                {
                    ModelState.AddModelError("", "Thay đổi thông tin thành công");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Thay đổi thông tin không thành công");
                }
            }

            return View(request);
        }

    }

}