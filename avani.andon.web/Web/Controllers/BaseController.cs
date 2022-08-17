using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace avSVAW.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lang = "";
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            if (session != null && session.CustomerId != null)
            {
                GlobalConstants.CUSTOMER_ID = Convert.ToInt32(session.CustomerId);
                ViewBag.ViewLines = new Model.Dao.LineDao().ListAllByCustomer(Convert.ToInt32(session.CustomerId), Convert.ToInt32(session.GroupId));
            }
            else
            {
                GlobalConstants.CUSTOMER_ID = 0;
            }
            var role = Convert.ToInt32(Session[GlobalConstants.ROLE_SESSION]);
            var supperadmin = false;
            var admin = false;
            var user = false;
            if (role == GlobalConstants.ROLE_SUPPER_ADMIN)
            {
                supperadmin = true;
            }
            else if (role == GlobalConstants.ROLE_ADMIN)
            {
                admin = true;
            }
            else
            {
                user = true;
            }
            ViewBag.IS_SA = supperadmin;
            ViewBag.IS_A = admin;
            ViewBag.IS_U = user;
            //else
            //{
            //    //phan quyen truy cap den controller va action (hien tai check phan quyen cho module User)
            //    string actionName = filterContext.ActionDescriptor.ActionName;
            //    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            //    string role = session.Role;
            //    if (session.Lang != null)
            //    {
            //        lang = session.Lang;
            //    }

            //    //if (role.Equals(GlobalConstants.MT_OFFICE_MEMBER_GROUP))
            //    //{
            //    //    if (controllerName.Equals("User") || 
            //    //        controllerName.Equals("Area1") || 
            //    //        controllerName.Equals("Area2") ||
            //    //        controllerName.Equals("Area2Nextway") ||
            //    //        controllerName.Equals("Area3") || 
            //    //        controllerName.Equals("Method"))
            //    //    {
            //    //        filterContext.Result = new RedirectToRouteResult(new
            //    //             RouteValueDictionary(new { controller = "Home", action = "Index" }));
            //    //    }
            //    //}

            //}
            var sessionLang = Session[GlobalConstants.LANG_SESSION];
            string culture = "vi-VN";
            if (sessionLang != null || lang !="")
            {
                if (lang == "")
                {
                    lang = Convert.ToString(sessionLang);
                }
                culture = string.Empty;
                if (lang.ToLower().CompareTo("vi") == 0 )
                {
                    culture = "vi-VN";
                }
                if (lang.ToLower().CompareTo("en") == 0)
                {
                    culture = "en-US";
                }

            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            base.OnActionExecuting(filterContext);
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}