using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Controllers
{
    public class ScheduleController : BaseController
    {
        // GET: Schedule
        public ActionResult Index()
        {
            List<tblSchedule> model = new ScheduleDao().ListAll();
            return View("Index", model);
        }
    }
}