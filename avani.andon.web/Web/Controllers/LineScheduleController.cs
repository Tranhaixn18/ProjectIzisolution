using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Controllers
{
    public class LineScheduleController : BaseController
    {
        // GET: LineSchedule
        public ActionResult Index()
        {
            List<tblLineSchedule> model = new LineScheduleDao().ListAll();
            return View("Index", model);
        }
    }
}