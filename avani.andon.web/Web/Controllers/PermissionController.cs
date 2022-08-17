using System.Web.Mvc;
using System.Configuration;
using System;
using System.Collections.Generic;
using Model.DataModel;
using Model.Dao;
using Common;
using System.Linq;
using avSVAW.Controllers;

namespace avEMS.Controllers
{
    public class PermissionController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        //Permission by Group
        public ActionResult Index(string GroupID)
        {
            if (GroupID == null) GroupID = "0";

            int _groupId = int.Parse(GroupID);
            tblUserGroup userGroup = new UserGroupDao().ViewDetail(_groupId);

            int _customerId = (int)userGroup.CustomerId;
            string GroupName = userGroup.Name;

            string ControlerBack = "/Groups/Index";

            List<tblUserPermission> lstPer = new UserPermissionDao().findByGroupID(_groupId);
            //List<tblNode> lstNode = new NodeDao().ListAllByCustomerId(_customerId);
            List<tblLine> lstLine = new LineDao().ListAllByCustomer(_customerId);


            //ViewBag.lstNode = lstNode;
            ViewBag.lstLine = lstLine;

            //ViewBag.Permission = lstPer;
            ViewBag.ControlerBack = ControlerBack;
            ViewBag.ObjectId = _groupId;
            ViewBag.ObjectName = GroupName;

            return View("Index", lstPer);
        }
        [HttpPost]
        public ActionResult Save()
        {
            string[] keys = Request.Form.AllKeys;

            int GroupId = Convert.ToInt32(Request.Form["ObjectId"]);
            string keyV = "";
            string keyU = "";

            List<tblUserPermission> lstPer = new UserPermissionDao().findByGroupID(GroupId).Where(x => x.ObjectType != GlobalConstants.MENU_OBJECT_TYPE).ToList();

            foreach (tblUserPermission p in lstPer)
            {
                if (p.ObjectType == GlobalConstants.LINE_OBJECT_TYPE)
                {
                    keyV = "VZ_" + p.ObjectId;
                    keyU = "UZ_" + p.ObjectId;
                }

                if (p.ObjectType == GlobalConstants.NODE_OBJECT_TYPE)
                {
                    keyV = "VN_" + p.ObjectId;
                    keyU = "UN_" + p.ObjectId;
                }

                p.View = (Request.Form[keyV] == "1");
                p.Update = (Request.Form[keyU] == "1");

                new UserPermissionDao().Update(p);

            }
            return Redirect("/Permission/Index?GroupId=" + GroupId);
        }

        [HttpGet]
        public ActionResult Object(int ObjectId, int ObjectType)
        {
            //Chỗ này list ra hết các Group
            if (ObjectType == GlobalConstants.MENU_OBJECT_TYPE)
            {
                //Chỗ này chỉ Supper Admin mới có
                //Tạm bỏ qua
            }

            string ControlerBack = "";
            string ObjectName = "";

            tblUser tblUser = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            int CustomerId = (int)tblUser.CustomerId;

            List<tblUserGroup> listGroups = new UserGroupDao().listAll(CustomerId);
            List<tblUserPermission> lstPer = new UserPermissionDao().findByObject(ObjectId, ObjectType);
            if (ObjectType == GlobalConstants.NODE_OBJECT_TYPE)
            {
                ControlerBack = "/Node/Index";
                ObjectName = new NodeDao().ViewDetail(ObjectId).Name;
            }

            if (ObjectType == GlobalConstants.LINE_OBJECT_TYPE)
            {
                ControlerBack = "/Line/Index";
                ObjectName = new LineDao().ViewDetail(ObjectId).Name;
            }

            ViewBag.Permission = lstPer;
            ViewBag.ObjectId = ObjectId;
            ViewBag.ObjectType = ObjectType;
            ViewBag.Name = ObjectName;

            ViewBag.ControlerBack = ControlerBack;
            return View("Object", listGroups);

        }

        [HttpPost]
        public ActionResult SaveObject()
        {
            string[] keys = Request.Form.AllKeys;
            int ObjectId = 0;
            int ObjectType = 0;

            ObjectId = Convert.ToInt32(Request.Form["ObjectId"]);
            ObjectType = Convert.ToInt32(Request.Form["ObjectType"]);

            //for (int i = 0; i < keys.Length; i++)
            //{
            //    if (keys[i].Contains("ObjectId"))
            //    {
            //    }

            //    if (keys[i].Contains("ObjectType"))
            //    {
            //    }

            //}
            string Param = "?ObjectId=" + ObjectId + "&ObjectType=" + ObjectType;

            List<tblUserPermission> lstPer = new UserPermissionDao().findByObject(ObjectId, ObjectType);

            foreach (tblUserPermission p in lstPer)
            {
                //bool CheckView = false;
                //bool CheckUpdate = false;

                string keyV = "V" + "_" + p.GroupId;
                string keyU = "U" + "_" + p.GroupId;

                p.View = (Request.Form[keyV] == "1");
                p.Update = (Request.Form[keyU] == "1");

                new UserPermissionDao().Update(p);

            }

            return Redirect("/Permission/PermissionByObject" + Param);
        }

    }
}
