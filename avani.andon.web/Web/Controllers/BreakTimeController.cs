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

namespace avSVAW.Controllers
{
    public class BreakTimeController : BaseController
    {
        
        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index()
        {
            List<tblBreakTime> model = new BreakTimeDao().listAll();
            return View("Index", model);
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create()
        {
            tblBreakTime model = new tblBreakTime();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(tblBreakTime model)
        {
            if (ModelState.IsValid)
            {
                new BreakTimeDao().Insert(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblBreakTime model = new BreakTimeDao().ViewDetail(Id);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(tblBreakTime model)
        {
            new BreakTimeDao().Update(model);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new BreakTimeDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblBreakTime> lst = new BreakTimeDao().listAll();
            return Json(lst);
        }
    }
}