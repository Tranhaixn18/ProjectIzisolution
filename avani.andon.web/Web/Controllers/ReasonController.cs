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
using avSVAW.Common;
using avSVAW.Models;
using avSVAW.App_Start;
using System.IO;
using System.Data.OleDb;

namespace avSVAW.Controllers
{
    public class ReasonController : BaseController
    {
        
        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index()
        {
            List<tblEventReason> model = new EventReasonDao().listAll();
            return View("Index", model);
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create()
        {
            ReasonForm model = new ReasonForm();
            List<tblEventReason> groups = new EventReasonDao().listByFilterGroup(0);
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (tblEventReason e in groups)
            {
                listItems.Add(new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                });
            }
            ViewBag.Groups = listItems;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReasonForm model)
        {
            if (ModelState.IsValid)
            {
                tblEventReason l = model.Cast();
                
                new EventReasonDao().Insert(l);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblEventReason l = new EventReasonDao().ViewDetail(Id);

            List<tblEventReason> groups = new EventReasonDao().listByFilterGroup(Id);

            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (tblEventReason e in groups)
            {
                listItems.Add(new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                });
            }
            ViewBag.Groups = listItems;

            ReasonForm model = new ReasonForm();
            model.Cast(l);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(ReasonForm model)
        {
            tblEventReason l = model.Cast();
            new EventReasonDao().Update(l);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new EventReasonDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblEventReason> lst = new EventReasonDao().listAll();
            return Json(lst);
        }

    }
}