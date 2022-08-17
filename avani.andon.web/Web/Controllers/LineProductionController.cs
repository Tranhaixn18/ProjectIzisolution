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
using System.Configuration;
using System.Data.SqlClient;
using Model.Models;
using System.Threading;
using System.Globalization;

namespace avSVAW.Controllers
{
    public class LineProductionController : Controller
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        public ActionResult Index(string isRefresh)
        {
            var sessionLang = Session[GlobalConstants.LANG_SESSION];
            string culture = "vi-VN";
            if (sessionLang != null)
            {
                string lang = Convert.ToString(sessionLang);
                culture = string.Empty;
                if (lang.ToLower().CompareTo("vi") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "vi-VN";
                }
                if (lang.ToLower().CompareTo("en") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "en-US";
                }

            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            List<tblEventDef> EventDef = new EventDefDao().listAll();
            //List<LineOnlineForm> Lines = new LineOnlineDao().LineJoinLineOnline();
            ViewBag.EventDefs = EventDef;

            DateTime dt = DateTime.Now;
            ViewBag.TimeNow = dt.ToString("HH:mm:ss");
            int hours = dt.Hour;
            List<tblShift> lstWorkShift = new WorkingShiftDao().listAll().ToList();
            string ShiftName = "";
            for (int i = 0; i < lstWorkShift.Count; i++)
            {
                if (lstWorkShift[i].StartHour < 22)
                {
                    if (lstWorkShift[i].StartHour < hours && lstWorkShift[i].FinishHour >= hours)
                    {
                        ShiftName = lstWorkShift[i].Name;
                    }
                }
                else
                {
                    if (lstWorkShift[i].StartHour <= hours || lstWorkShift[i].FinishHour > hours)
                    {
                        ShiftName = lstWorkShift[i].Name;
                    }
                }
            }
            ViewBag.ShiftName = ShiftName;
            ViewBag.WorkShifts = lstWorkShift;

            return View("Index"/*, Lines*/);
        }
       
    }
}