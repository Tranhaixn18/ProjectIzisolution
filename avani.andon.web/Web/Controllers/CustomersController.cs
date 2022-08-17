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
using avEMS.Controllers;
using System.IO;
using avSVAW.Models;

namespace avSVAW.Controllers
{
    public class CustomersController : BaseController
    {

        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index()
        {
            if (!ViewBag.IS_SA)
            {
                return RedirectToAction("Index", "Home");
            }
            List<tblCustomer> lstCustomer = new CustomerDao().listAll();
            List<CustomerForm> model = new List<CustomerForm>();
            foreach (tblCustomer g in lstCustomer)
            {
                CustomerForm f = new CustomerForm();
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
            CustomerForm n = new CustomerForm();
            return View(n);
        }

        [HttpPost]
        public ActionResult Create(CustomerForm model)
        {
            tblCustomer g = model.cast();
            var file = model.LogoUpload;
            string random = Encryptor.CreateRandomPassword(5);
            string strDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _fileName = random + "_" + strDate + Path.GetExtension(file.FileName);
            string _path = Path.Combine(Server.MapPath("~/Uploads/Logo"), _fileName);
            file.SaveAs(_path);
            g.Logo = _fileName;
            new CustomerDao().Insert(g);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblCustomer l = new CustomerDao().ViewDetail(Id);
            CustomerForm g = new CustomerForm();
            g.cast(l);
            return View(g);
        }


        [HttpPost]
        public ActionResult Edit(CustomerForm model)
        {
            tblCustomer g = model.cast();
            var file = model.LogoUpload;
            if (file != null)
            {
                string random = Encryptor.CreateRandomPassword(5);
                string strDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                string _fileName = random + "_" + strDate + "." + Path.GetExtension(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Uploads/Logo"), _fileName);
                file.SaveAs(_path);
                g.Logo = _fileName;
            }
            new CustomerDao().Update(g);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new CustomerDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblCustomer> lst = new CustomerDao().listAll();
            return Json(lst);
        }
    }
}