using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Model.DataModel;
using Model.Dao;
using Common;
using System.Collections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace avSVAW.Controllers
{
    [Route("api/{controller}/{action}")]
    public class ApiRFIDController: ApiController
    {

        private string _GetIPAddress()
        {
            HttpContext context = HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        [HttpPost]
        [Route("api/ApiRFID/ApiLogin")]
        public ArrayList ApiLogin()
        {
            ArrayList result = new ArrayList();
            var value = new Dictionary<string, string>();
            try
            {
                var stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(stream).ReadToEnd();
                value = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                result.Add(false);
                result.Add("Json không đúng format!!!");
                return result;
            }
            string username;
            string password;
            value.TryGetValue("username", out username);
            value.TryGetValue("password", out password);

            var dao = new UserDao();
            tblUser user = dao.GetBytblUserName(username);

            string passMd5 = Encryptor.MD5Hash(password);
            var resultLogin = dao.Login(username, passMd5);
            if (resultLogin == 1)
            {
                if (user.Role == GlobalConstants.AV_ADMIN_GROUP)
                {
                    string timeStamp = DateTime.Now.ToString();
                    string token = Encryptor.MD5Hash(password + "." + timeStamp);
                    DateTime today = DateTime.Now;
                    DateTime expireDate = DateTime.Now.AddHours(24);
                    string ipClient = this._GetIPAddress();
                    tblUserLogin userLogin = new tblUserLogin()
                    {
                        User_Id = user.ID,
                        Ip = ipClient,
                        Expire_Date = expireDate,
                        Token = token,
                        Time_Login = today,
                        State = true
                    };
                    var userLoginDao = new Model.Dao.UserLoginDao();
                    userLoginDao.ChangeStatus(user.ID);
                    userLoginDao.Create(userLogin);
                    result.Add(true);
                    result.Add(token);
                    return result;
                }
                else
                {
                    result.Add(false);
                    result.Add("Đối tượng không được truy cập vào nhân viên!");
                    return result;
                }

            }
            else if (resultLogin == 0)
            {
                result.Add(false);
                result.Add("Tài khoản không tồn tại.");
                return result;
            }
            else if (resultLogin == -1)
            {
                result.Add(false);
                result.Add("Tài khoản đang bị khoá.");
                return result;
            }
            else if (resultLogin == -2)
            {
                result.Add(false);
                result.Add("Mật khẩu không đúng.");
                return result;
            }
            else if (resultLogin == -3)
            {
                result.Add(false);
                result.Add("Tài khoản của bạn không có quyền đăng nhập.");
                return result;
            }
            else
            {
                result.Add(false);
                result.Add("Có lỗi đăng nhập.");
                return result;
            }

        }

        [Route("api/ApiRFID/CreateEmployee")]
        [HttpPost]
        public ArrayList CreateEmployee()
        {
            ArrayList result = new ArrayList();
            var value = new Dictionary<string, string>();
            try
            {
                var stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(stream).ReadToEnd();
                value = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                result.Add(false);
                result.Add("Json không đúng format!!!");
                return result;
            }
            string token;
            string tmp_is_new;
            value.TryGetValue("token", out token);
            value.TryGetValue("is_new", out tmp_is_new);
            bool is_new = bool.Parse(tmp_is_new);
            var userLoginDao = new UserLoginDao();
            var validToken = userLoginDao.checkToken(token);
            if (validToken)
            {
                string code;
                string name;
                string depart;
                string phone;
                string card;
                string email;
                string description;
                string tmp_active;
                string id;
                //get data from json
                value.TryGetValue("code", out code);
                value.TryGetValue("name", out name);
                value.TryGetValue("depart", out depart);
                value.TryGetValue("phone", out phone);
                value.TryGetValue("card", out card);
                value.TryGetValue("email", out email);
                value.TryGetValue("description", out description);
                value.TryGetValue("active", out tmp_active);
                // action create or update employee
                tblEmployee emp;
                var employeeDao = new EmployeeDao();

                if (!is_new)
                {
                    value.TryGetValue("id", out id);
                    emp = new tblEmployee()
                    {
                        Id = Int32.Parse(id),
                        Code = code,
                        Name = name,
                        Phone = phone,
                        CardId = card,
                        DeptId = Int32.Parse(depart),
                        Email = email,
                        Description = description,
                      //  Active = bool.Parse(tmp_active)

                    };
                    try
                    {
                        employeeDao.Update(emp);
                        result.Add(true);
                        result.Add("Cập nhật nhân viên mới thành công");
                    }
                    catch (Exception er)
                    {
                        result.Add(false);
                        result.Add("Cập nhật nhân viên mới thất bại. Vui lòng kiểm tra lại hệ thống.");
                    }
                }
                else
                {
                    emp = new tblEmployee()
                    {
                        Code = code,
                        Name = name,
                        Phone = phone,
                        CardId = card,
                        DeptId = Int32.Parse(depart),
                        Email = email,
                        Description = description,
                       // Active = bool.Parse(tmp_active)

                    };
                    try
                    {
                        employeeDao.Insert(emp);
                        result.Add(true);
                        result.Add("Tạo nhân viên mới thành công");
                    }
                    catch (Exception er)
                    {
                        result.Add(false);
                        result.Add("Tạo nhân viên mới thất bại. Vui lòng kiểm tra lại hệ thống.");
                    }
                }
                
                
                return result;
            }
            else
            {
                result.Add(false);
                result.Add("Token không hợp lệ!!!");
                return result;
            }
        }

        [Route("api/ApiRFID/DeleteEmployee")]
        [HttpPost]
        public ArrayList DeleteEmployee()
        {
            ArrayList result = new ArrayList();
            var value = new Dictionary<string, string>();
            try
            {
                var stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(stream).ReadToEnd();
                value = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                result.Add(false);
                result.Add("Json không đúng format!!!");
                return result;
            }
            string token;
            value.TryGetValue("token", out token);
            var userLoginDao = new UserLoginDao();
            var validToken = userLoginDao.checkToken(token);
            if (validToken)
            {
                string id;
                //get data from json
                value.TryGetValue("id", out id);
                // action create or update employee
                tblEmployee emp;
                var employeeDao = new EmployeeDao();
                try
                {
                    employeeDao.Delete(Int32.Parse(id));
                    result.Add(true);
                    result.Add("Xóa nhân viên thành công");
                }
                catch (Exception er)
                {
                    result.Add(false);
                    result.Add("Xóa nhân viên thất bại. Vui lòng kiểm tra lại hệ thống.");
                }
                return result;
            }
            else
            {
                result.Add(false);
                result.Add("Token không hợp lệ!!!");
                return result;
            }
        }



        [Route("api/ApiRFID/ListEmployee")]
        [HttpPost]
        public ArrayList ListEmployee()
        {
            ArrayList result = new ArrayList();
            var value = new Dictionary<string, string>();
            try
            {
                var stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(stream).ReadToEnd();
                value = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                result.Add(false);
                result.Add("Json không đúng format!!!");
                return result;
            }
            string token;
            value.TryGetValue("token", out token);

            var userLoginDao = new UserLoginDao();
            var validToken = userLoginDao.checkToken(token);
            if (validToken)
            {
                result.Add(true);
                var listEmployees = userLoginDao.getEmployees();
                result.Add(listEmployees);
                return result;
            }
            else
            {
                result.Add(false);
                result.Add("Token không hợp lệ!!!");
                return result;
            }
        }
        
        
        [Route("api/ApiRFID/ListDepartment")]
        [HttpPost]
        public ArrayList ListDepartment()
        {
            ArrayList result = new ArrayList();
            var value = new Dictionary<string, string>();
            try
            {
                var stream = HttpContext.Current.Request.InputStream;
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(stream).ReadToEnd();
                value = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                result.Add(false);
                result.Add("Json không đúng format!!!");
                return result;
            }
            string token;
            value.TryGetValue("token", out token);

            var userLoginDao = new UserLoginDao();
            var validToken = userLoginDao.checkToken(token);
            if (validToken)
            {
                result.Add(true);
                var listDepartment = userLoginDao.getDepartments();
                result.Add(listDepartment);
                return result;
            }
            else
            {
                result.Add(false);
                result.Add("Token không hợp lệ!!!");
                return result;
            }
        }

        [Route("api/ApiRFID/GetRFID")]
        [HttpGet]
        public string GetRFID()
        {
            ArrayList result = new ArrayList();
            string res = App_Start.WebApiConfig.openListenRabbitMQ();
            Debug.WriteLine(res);
            return res;
        }
    }
}