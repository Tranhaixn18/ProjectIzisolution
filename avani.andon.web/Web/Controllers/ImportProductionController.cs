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
    public class ImportProductionController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        /*public ActionResult Index(string strDate, string LineId)
        {

            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                try
                {
                    iLineId = Convert.ToInt32(LineId);
                }
                catch { }
            }
            ViewBag.LineId = LineId;

            List<tblLine> lstLines = new LineDao().listAll().ToList(); // 1 ~ id Ca HC
            List<SelectListItem> Lines = new List<SelectListItem>();
            foreach (var _l in lstLines)
            {
                Lines.Add(new SelectListItem() { Value = _l.Id.ToString(), Text = _l.Name.ToString() });
            }
            Lines.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "0" });
            ViewBag.Lines = Lines;
            ViewBag.lstLines = lstLines;
           

            DateTime dDate = DateTime.Now;
            if (!string.IsNullOrEmpty(strDate))
            {
                dDate = Convert.ToDateTime(strDate);
            }
            ViewBag.strDate = dDate.ToString("yyyy/MM/dd");

            List<tblWorkPlan> plans = new WorkingPlanDao().ListByKey(dDate.Year, dDate.Month, dDate.Day, iLineId);
            //foreach(tblWorkingPlan wp in plans)
            //{
            //    List<tblLineProduction> lp = new LineProducttionDao().listAll(wp.Id, iLineId);
            //}
            return View("Index", plans);
        }*/
        [WebMethod]
        public JsonResult UpdateQuality(string Id,string LineId, string Quality, string NG)
        {
            // load workshift
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }

            using (SqlConnection con = new SqlConnection(strConStr))
            {

                con.Open();
                if (!string.IsNullOrEmpty(Id))
                {
                    double _quality = 0;
                    int _ng = 0;
                    if (!string.IsNullOrEmpty(NG))
                    {
                        _ng = Convert.ToInt32(NG);
                    }
                    if (!string.IsNullOrEmpty(Quality))
                    {
                        _quality = Convert.ToInt32(Quality);
                    }
                    //Nếu là ngày hôm nay thì cho nó cập nhật lại phần sự kiện trước khi lấy.
                    //exec [CreateEventReport] @Year = 2019, @Month = 7, @Day = 23, @Hour2Update = 6
                    string query = "exec [dbo].[UpdateQuality]  @LineId = " + iLineId + ", @WorkingPlanId = " + Id + ",@Quality="+ _quality + ",@NG="+ _ng;

                    SqlCommand CreateCmd = new SqlCommand(query, con);
                    CreateCmd.ExecuteNonQuery();

                    string query1 = "UPDATE [dbo].[tblWorkingPlan]  SET Quality = " + _quality + ",NG="+_ng+" WHERE Id=" + Id;

                    SqlCommand CreateCmd1 = new SqlCommand(query1, con);
                    CreateCmd1.ExecuteNonQuery();
                }
                
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
    
}