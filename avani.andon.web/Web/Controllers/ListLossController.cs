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
    public class ListLossController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        /*public ActionResult Index()
        {
            DateTime _today = DateTime.Now;

            if (_today.Hour < Hour2UpdateReportDaily)
            {
                _today = _today.AddDays(-1);
            }
            DateTime _tomorrow = _today.AddDays(1);

            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            List<tblNodeEvent> lstNEvent = new NodeEventDao().listByDate(
                _today.Year, _today.Month, _today.Day, Hour2UpdateReportDaily, 0,
                _tomorrow.Year, _tomorrow.Month, _tomorrow.Day, Hour2UpdateReportDaily, 0
                );
            List<tblEventDef> lstEventDef = new EventDefDao().listAll();
            ViewBag.EventDefs = lstEventDef;
            List<tblEventReason> lstReasons = new EventReasonDao().listAll();

            var groupReason = lstReasons.GroupBy(item => item.GroupName).Select(group => new GroupReasonForm { GroupName = group.Key, Items = group.ToList() } ).ToList();
            ViewBag.GroupReason = groupReason;


            List<ListLossForm> models = new List<ListLossForm>();
            foreach(tblNodeEvent nE in lstNEvent)
            {
                ListLossForm llF = new ListLossForm();
                llF.Cast(nE);
                if (UserFunction.USER_LINE_LEADER(session.Role))
                {
                    if (session.LineId == llF.LineId)
                    {
                        models.Add(llF);
                    }
                }
                else
                {
                    models.Add(llF);
                }
            }
            return View("Index", models);
        }*/
       /* [WebMethod]*/
/*        public JsonResult GetLineId(string Id)
        {
            int _Id = 0;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
           
            if (!string.IsNullOrEmpty(Id))
            {
                List<tblShift> lstWorkShift = new WorkingShiftDao().listAll().ToList();
                
                _Id = Convert.ToInt32(Id);
                tblNodeEvent nE = new NodeEventDao().ViewDetail(_Id);
                ListLossForm form = new ListLossForm();
                form.Cast(nE);
                DateTime dt = DateTime.Now;
                int hoursNow = dt.Hour;
                DateTime DT3 = Convert.ToDateTime(form.T3);
                bool ok = false;
              
                int startyear = dt.Year;
                int startmonth = dt.Month;
                int startday = dt.Day;
                int endYear = dt.Year;
                int endMonth = dt.Month;
                int endDay = dt.Day;
                int hours = dt.Hour;
                int startHours = 0;
                int endHours = 0;
                int startMinute = 0;
                int endMinute = 0;
               
                for (int i = 0; i < lstWorkShift.Count; i++)
                {
                    if (lstWorkShift[i].StartHour >= 6 && lstWorkShift[i].StartHour < 22)
                    {
                        DateTime startD = new DateTime(startyear, startmonth, startday, lstWorkShift[i].StartHour, lstWorkShift[i].StartMinute, 0);
                        DateTime endD = new DateTime(startyear, startmonth, startday, lstWorkShift[i].FinishHour, lstWorkShift[i].FinishMinute, 0);
                        if (startD.Ticks < dt.Ticks && endD.Ticks > dt.Ticks)
                        {
                            startHours = lstWorkShift[i].StartHour;
                            endHours = lstWorkShift[i].FinishHour;
                            startMinute = lstWorkShift[i].StartMinute;
                            endMinute = lstWorkShift[i].FinishMinute;
                        }
                    }
                    else
                    {
                        if (lstWorkShift[i].StartHour <= hours || lstWorkShift[i].FinishHour > hours)
                        {
                            if (lstWorkShift[i].FinishHour > hours)
                            {
                                dt = dt.AddDays(-1);
                                startyear = dt.Year;
                                startmonth = dt.Month;
                                startday = dt.Day;
                            }
                            else
                            {
                                dt = dt.AddDays(1);
                                endYear = dt.Year;
                                endMonth = dt.Month;
                                endDay = dt.Day;

                            }
                            startHours = lstWorkShift[i].StartHour;
                            endHours = lstWorkShift[i].FinishHour;
                            startMinute = lstWorkShift[i].StartMinute;
                            endMinute = lstWorkShift[i].FinishMinute;
                        }
                    }
                }
                DateTime startDate = new DateTime(startyear, startmonth, startday, startHours, startMinute, 0);
                DateTime endDate = new DateTime(endYear, endMonth, endDay, endHours, endMinute, 0);

                if (DT3>startDate && DT3<endDate)
                {

                    if (UserFunction.USER_LINE_LEADER(session.Role))
                    {
                        if (session.LineId == form.LineId)
                        {
                            return Json(form, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("False", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(form, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
           
        }*/
        /*[WebMethod]*/
       /* public JsonResult GetData()
        {
            DateTime dt = DateTime.Now;
            int startyear = dt.Year;
            int startmonth = dt.Month;
            int startday = dt.Day;
            int endYear = dt.Year;
            int endMonth = dt.Month;
            int endDay = dt.Day;
            int hours = dt.Hour;
            int startHours = 0;
            int endHours = 0;
            int startMinute = 0;
            int endMinute = 0;
            List<tblShift> lstWorkShift = new WorkingShiftDao().listAll().ToList();

            for (int i = 0; i < lstWorkShift.Count; i++)
            {
                if (lstWorkShift[i].StartHour >= 6 && lstWorkShift[i].StartHour < 22)
                {
                    DateTime startD = new DateTime(startyear, startmonth, startday, lstWorkShift[i].StartHour, lstWorkShift[i].StartMinute, 0);
                    DateTime endD = new DateTime(startyear, startmonth, startday, lstWorkShift[i].FinishHour, lstWorkShift[i].FinishMinute, 0);
                    if (startD.Ticks < dt.Ticks && endD.Ticks > dt.Ticks)
                    {
                        startHours = lstWorkShift[i].StartHour;
                        endHours = lstWorkShift[i].FinishHour;
                        startMinute = lstWorkShift[i].StartMinute;
                        endMinute = lstWorkShift[i].FinishMinute;
                    }
                }
                else
                {
                    if (lstWorkShift[i].StartHour <= hours || lstWorkShift[i].FinishHour > hours)
                    {
                        if (lstWorkShift[i].FinishHour > hours)
                        {
                            dt = dt.AddDays(-1);
                            startyear = dt.Year;
                            startmonth = dt.Month;
                            startday = dt.Day;
                        }
                        else
                        {
                            dt = dt.AddDays(1);
                            endYear = dt.Year;
                            endMonth = dt.Month;
                            endDay = dt.Day;

                        }
                        startHours = lstWorkShift[i].StartHour;
                        endHours = lstWorkShift[i].FinishHour;
                        startMinute = lstWorkShift[i].StartMinute;
                        endMinute = lstWorkShift[i].FinishMinute;
                    }
                }
            }

            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            List<tblNodeEvent> lstNEvent = new NodeEventDao().listByDate(
                startyear, startmonth, startday, startHours, startMinute,
                endYear, endMonth, endDay, endHours, endMinute
                );
            List<tblEventDef> lstEventDef = new EventDefDao().listAll();
            ViewBag.EventDefs = lstEventDef;
            List<ListLossForm> models = new List<ListLossForm>();
            foreach (tblNodeEvent nE in lstNEvent)
            {
                ListLossForm llF = new ListLossForm();
                llF.Cast(nE);
                if (UserFunction.USER_LINE_LEADER(session.Role))
                {
                    if (session.LineId == llF.LineId)
                    {
                        models.Add(llF);
                    }
                }
                else
                {
                    models.Add(llF);
                }
            }
            return Json(models, JsonRequestBehavior.AllowGet);
        }*/

        [HttpPost]
        public JsonResult UpdateReason(string Id, string Reason)
        {
            int _Id = 0;
            string _Reason = "";

            if (!string.IsNullOrEmpty(Id))
            {
                _Id = int.Parse(Id); ;
            }
            if (!string.IsNullOrEmpty(Reason))
            {
                _Reason = Reason;
            }
            if (_Id != 0 && _Reason !="") //Đây là update rồi
            {
                //Xóa bỏ các loại trong bảng đi
                //new WorkingPlanDao().DeleteWorking(_Id);
                //Xóa bằng lệnh cho nhanh
                using (SqlConnection con = new SqlConnection(strConStr))
                {
                    con.Open();
                    string query = "UPDATE [tblNodeEvent] SET Reason=N'" + Reason+"' FROM [tblNodeEvent] WHERE Id = " + _Id;
                    SqlCommand CreateCmd = new SqlCommand(query, con);
                    CreateCmd.ExecuteNonQuery();
                }
            }
           
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}