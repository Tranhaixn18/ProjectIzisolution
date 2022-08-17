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
    public class UpdateOnlineController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        /*public ActionResult Index()
        {
            List<tblLineOnline> LineOnlines = new LineOnlineDao().listAll();


            return View("Index", LineOnlines );
        }
        public ActionResult Edit(string LineId)
        {
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            tblLineOnline LineOnlines = new LineOnlineDao().ViewDetail(iLineId);


            return View("Edit", LineOnlines);
        }


        [HttpPost]
        public ActionResult Edit(tblLineOnline line)
        {

            tblLineOnline lineOnline = new LineOnlineDao().ViewDetail(line.LineId);
            lineOnline.LineName = line.LineName;
            lineOnline.Plan = line.Plan;
            lineOnline.StartTime = line.StartTime;
            lineOnline.Actual = line.Actual;
            lineOnline.StopDuration = line.StopDuration;
            lineOnline.TakeTime = line.TakeTime;
            lineOnline.HeadCount = line.HeadCount;
            lineOnline.Quality= line.Quality;
            new LineOnlineDao().Update(lineOnline);
            return RedirectToAction("Index");
        }*/


    }
}