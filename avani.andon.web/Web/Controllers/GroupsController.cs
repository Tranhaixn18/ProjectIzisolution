using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Common;
using Model.Dao;
using Model.DataModel;
using avSVAW.Controllers;
using avSVAW.Models;

namespace avEMS.Controllers
{
    public class GroupsController : BaseController
    {

        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index()
        {
            List<tblUserGroup> lstGroup = new List<tblUserGroup>();
            var group = (tblUserGroup)Session[GlobalConstants.GROUP_SESSION];
            if (group.CustomerId==0)
            {
                lstGroup = new UserGroupDao().listAll();
            }
            else
            {
                int CustomerID = Convert.ToInt32(group.CustomerId);
                lstGroup = new UserGroupDao().listAll(CustomerID);
            }
            List<GroupForm> model = new List<GroupForm>();
            foreach (tblUserGroup g in lstGroup)
            {
                GroupForm f = new GroupForm();
                f.cast(g);
                model.Add(f);
            }
            return View("Index", model);
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create()
        {
            var group = (tblUserGroup)Session[GlobalConstants.GROUP_SESSION];
            ViewBag.CustomerId = Convert.ToInt32(group.CustomerId);


            GroupForm n = new GroupForm();

            List<tblUserRole> roles = new UserRoleDao().listAll();

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (tblUserRole role in roles)
            {
                if (Convert.ToBoolean(ViewBag.ISVI))
                {
                    list.Add(new SelectListItem() { Value = role.Code, Text = role.Name_VN });
                }
                else
                {
                    list.Add(new SelectListItem() { Value = role.Code, Text = role.Name_EN });
                }
            }
            n.Roles = list;

            return View(n);
        }

        [HttpPost]
        public ActionResult Create(GroupForm model)
        {
            tblUserGroup g = model.cast();
            new UserGroupDao().Insert(g);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblUserGroup l = new UserGroupDao().ViewDetail(Id);
            GroupForm g = new GroupForm();
            g.cast(l);

            List<tblUserRole> roles = new UserRoleDao().listAll();

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (tblUserRole role in roles)
            {
                if (Convert.ToBoolean(ViewBag.ISVI))
                {
                    list.Add(new SelectListItem() { Value = role.Code, Text = role.Name_VN });
                }
                else
                {
                    list.Add(new SelectListItem() { Value = role.Code, Text = role.Name_EN });
                }
            }

            g.Roles = list;

            return View(g);
        }


        [HttpPost]
        public ActionResult Edit(GroupForm model)
        {
            tblUserGroup g = model.cast();
            new UserGroupDao().Update(g);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new UserGroupDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblUserGroup> lst = new UserGroupDao().listAll();
            return Json(lst);
        }
    }
}