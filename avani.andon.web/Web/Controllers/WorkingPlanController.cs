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
using System.Data.SqlClient;
using System.Configuration;
using Model.Models;

namespace avSVAW.Controllers
{
    public class WorkingPlanController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index(string Year, string Month, string Shift, string LineId, string Status)
        {
            int iYear = DateTime.Now.Year, iMonth = DateTime.Now.Month;
            int iShift = 0;
            int iStatus = -1;
            string strNode = "";

            if (!string.IsNullOrEmpty(Year))
            {
                iYear = Convert.ToInt32(Year);
            }
            if (!string.IsNullOrEmpty(Status))
            {
                iStatus = Convert.ToInt32(Status);
            }
            if (!string.IsNullOrEmpty(Month))
            {
                iMonth = Convert.ToInt32(Month);
            }


            if (!string.IsNullOrEmpty(Shift))
            {
                iShift = Convert.ToInt32(Shift);
            }

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


            List<tblNode> lstNode = new NodeDao().listAll().ToList(); // 1 ~ id Ca HC
            List<SelectListItem> Nodes = new List<SelectListItem>();
            foreach (var _l in lstNode)
            {
                Nodes.Add(new SelectListItem() { Value = _l.Id.ToString(), Text = _l.Name.ToString() });
            }
            Nodes.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "0" });
            ViewBag.lstNode = Nodes;


            ViewBag.Year = iYear;
            ViewBag.Month = iMonth;

            ViewBag.Shift = iShift;
            ViewBag.Node = strNode;
            ViewBag.Statuss = iStatus;

            List<SelectListItem> Years = new List<SelectListItem>();
            for (int i = ViewBag.Year; i <= ViewBag.Year + 1; i++)
            {
                Years.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Years = Years;

            List<SelectListItem> Months = new List<SelectListItem>();
            for (int i = 0; i <= 12; i++)
            {
                Months.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Months = Months;
            List<SelectListItem> listStatus = new List<SelectListItem>();
            for (int i = -1; i < 6; i++)
            {
                if (i == -1) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "-- " + @Resources.Language.All + "--" });
                else if (i == 0) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Kế hoạch" });
                else if (i == 1) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Đang chạy" });
                else if (i == 2) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Tạm dừng" });
                else if (i == 3) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Hoàn thành" });
                else if (i == 4) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Quá hạn" });
                else if (i == 5) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Hủy bỏ" });
            }
            ViewBag.Status = listStatus;

            List<SelectListItem> Shifts = new List<SelectListItem>();
            List<tblShift> shifts = new WorkingShiftDao().listAll();
            foreach (var s in shifts)
            {
                string StartHour = s.StartHour < 10 ? "0" + s.StartHour : s.StartHour.ToString();
                string StartMinute = s.StartMinute < 10 ? "0" + s.StartMinute : s.StartMinute.ToString();
                string FinishHour = s.FinishHour < 10 ? "0" + s.FinishHour : s.FinishHour.ToString();
                string FinishMinute = s.FinishMinute < 10 ? "0" + s.FinishMinute : s.FinishMinute.ToString();
                Shifts.Add(new SelectListItem() { Value = s.Id.ToString(), Text = s.Name + " [" + StartHour + ":" + StartMinute + "-" + FinishHour + ":" + FinishMinute + "]" });
            }
            Shifts.Insert(0, new SelectListItem { Text = "-- " + @Resources.Language.All + " --", Value = "" });

            ViewBag.Shifts = Shifts;

            List<SelectListItem> BreakTimes = new List<SelectListItem>();
            List<tblBreakTime> _BreakTimes = new BreakTimeDao().listAll();
            foreach (var s in _BreakTimes)
            {
                string StartHour = s.StartHour < 10 ? "0" + s.StartHour : s.StartHour.ToString();
                string StartMinute = s.StartMinute < 10 ? "0" + s.StartMinute : s.StartMinute.ToString();
                string FinishHour = s.FinishHour < 10 ? "0" + s.FinishHour : s.FinishHour.ToString();
                string FinishMinute = s.FinishMinute < 10 ? "0" + s.FinishMinute : s.FinishMinute.ToString();
                Shifts.Add(new SelectListItem() { Value = s.Id.ToString(), Text = s.Name + " [" + StartHour + ":" + StartMinute + "-" + FinishHour + ":" + FinishMinute + "]" });
            }
            BreakTimes.Insert(0, new SelectListItem { Text = "-- " + @Resources.Language.All + " --", Value = "" });

            ViewBag.BreakTimes = BreakTimes;

            List<WorkPlanForm> model = new List<WorkPlanForm>();
            List<tblWorkPlan> l = new List<tblWorkPlan>();
            Log.Debug("Start Query in :" + DateTime.Now);
            using (SqlConnection con = new SqlConnection(strConStr))
            {
                string strQuery = "";
                con.Open();
                if (iLineId > 0 && iStatus == -1)
                {
                    strQuery = "SELECT * FROM tblWorkPlan WHERE MONTH(PlanStart) = '" + iMonth.ToString() + "' AND YEAR(PlanStart) = '" + iYear + " AND LineId = " + iLineId + "ORDER BY Id DESC";
                }
                if (iLineId == 0 && iStatus > -1)
                {
                    strQuery = "SELECT * FROM tblWorkPlan WHERE MONTH(PlanStart) = '" + iMonth.ToString() + "' AND YEAR(PlanStart) = '" + iYear + " AND Status = " + iStatus + "ORDER BY Id DESC";
                }
                if (iLineId > 0 && iStatus > -1)
                {
                    strQuery = "SELECT * FROM tblWorkPlan WHERE MONTH(PlanStart) = '" + iMonth.ToString() + "' AND YEAR(PlanStart) = '" + iYear + " AND Status = " + iStatus + " AND LineId = " + iLineId + "ORDER BY Id DESC";
                }
                if (iLineId == 0 && iStatus == -1)
                {
                    strQuery = "SELECT * FROM tblWorkPlan WHERE MONTH(PlanStart) = '" + iMonth.ToString() + "' AND YEAR(PlanStart) = '" + iYear + "'" + "ORDER BY Id DESC";
                }

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    var wp = new WorkPlanForm();
                    wp.Id = Convert.ToInt64(dr["Id"]);
                    wp.LineNames = new LineDao().ViewDetailSql(dr["LineCode"].ToString());
                    wp.PlanStart = Convert.ToDateTime(dr["PlanStart"].ToString());
                    wp.PlanFinish = Convert.ToDateTime(dr["PlanFinish"].ToString());
                    if (dr["PlanQuantity"] is DBNull) { wp.PlanQuantity = 0; }
                    else { wp.PlanQuantity = Convert.ToInt32(dr["PlanQuantity"]); }


                    wp.WorkOrderQuantity = new WorkOrderPlanDao().getByWorkPlanIdSQL(Convert.ToInt64(dr["Id"]));


                    wp.Status = (byte?)(dr["Status"] is DBNull ? 1 : Convert.ToByte(dr["Status"]));
                    model.Add(wp);
                }
            }

            //List<tblWorkPlan> plans = new WorkPlanDao().ListAllByFilter(iLineId, iYear, iMonth, iStatus);
            //foreach (tblWorkPlan plan in plans)
            //{
            //    WorkPlanForm f = new WorkPlanForm();
            //    f.Cast(plan);
            //    model.Add(f);
            //}
            Log.Debug("End Query in:" + DateTime.Now);
            return View("Index", model);
        }

        public ActionResult WorkingPlanDetail(string Id, string LineId, string NodeId, string Year, string Month)
        {
            int iYear = DateTime.Now.Year, iMonth = DateTime.Now.Month;
            if (!string.IsNullOrEmpty(Year))
            {
                iYear = Convert.ToInt32(Year);
            }
            if (!string.IsNullOrEmpty(Month))
            {
                iMonth = Convert.ToInt32(Month);
            }

            int iId = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                iId = Convert.ToInt32(Id);
            }

            ViewBag.WorkPlanId = iId;

            ViewBag.LineId = LineId;
            ViewBag.NodeId = NodeId;

            ViewBag.Year = iYear;
            ViewBag.Month = iMonth;


            tblWorkPlan workPlan = new WorkPlanDao().ViewDetail(iId);
            WorkPlanForm planForm = new WorkPlanForm();
            planForm.Cast(workPlan);
            ViewBag.WorkPlanForm = planForm;
            List<tblWorkOrderPlan> lstWOP = new WorkOrderPlanDao().listAllByFilter2(iId).OrderBy(x => x.nOrder).ToList();
            List<WorkOrderPlanForm> model = new List<WorkOrderPlanForm>();
            foreach (tblWorkOrderPlan wop in lstWOP)
            {
                WorkOrderPlanForm wopf = new WorkOrderPlanForm();
                wopf.Cast(wop);
                model.Add(wopf);
            }
            return View("WorkingPlanDetail", model);
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create(string Year = "", string Month = "")
        {
            ViewBag.Id = 0;
            int iYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(Year))
            {
                iYear = Convert.ToInt32(Year);
            }
            int iMonth = DateTime.Now.Month;
            if (!string.IsNullOrEmpty(Month))
            {
                iMonth = Convert.ToInt32(Month);
            }
            ViewBag.Year = iYear;
            ViewBag.Month = iMonth;

            List<SelectListItem> Years = new List<SelectListItem>();
            for (int i = ViewBag.Year; i <= ViewBag.Year + 1; i++)
            {
                Years.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Years = Years;

            List<SelectListItem> Months = new List<SelectListItem>();
            for (int i = 0; i <= 12; i++)
            {
                Months.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Months = Months;

            ViewBag.Days = DateTime.DaysInMonth(ViewBag.Year, ViewBag.Month);
            ViewBag.Shifts = new WorkingShiftDao().listAll();
            ViewBag.Lines = new LineDao().listAll();
            ViewBag.Nodes = new NodeDao().listAll();
            //ViewBag.NodeTypes = new NodeTypeDao().listAll(true);
            List<tblBreakTime> _BreakTimes = new BreakTimeDao().listAll();

            ViewBag.BreakTimes = _BreakTimes;
            tblWorkPlan model = new tblWorkPlan();
            //model.Year = ViewBag.Year;
            ViewBag.HeadCount = "";
            ViewBag.Quality = "";
            ViewBag.TaktTime = "";
            return View();
        }

        [HttpPost]
        public ActionResult Create(tblWorkPlan model)
        {
            if (ModelState.IsValid)
            {
                new WorkingPlanDao().Insert(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int Id, string LineId, string WorkingId)
        {
            ViewBag.LineId = LineId;
            ViewBag.WorkingId = WorkingId;
            tblWorkPlan model = new WorkingPlanDao().ViewDetail(Id);
            return View("Edit", model);
        }

        [HttpPost]
        /*public ActionResult Edit(tblWorkPlan tblPlan)
        {
            if (ModelState.IsValid)
            {
                bool isEdit = checkWorkingPlan(tblPlan.LineId.ToString(), tblPlan.Year, tblPlan.Month, tblPlan.Day, tblPlan.StartHour, tblPlan.StartMinute, tblPlan.FinishHour, tblPlan.FinishMinute);
                if (isEdit)
                {
                    var planDao = new WorkingPlanDao();
                    var result = planDao.Update(tblPlan);
                    if (result)
                    {
                        return RedirectToAction("WorkingPlanDetail", "WorkingPlan", new { Year = tblPlan.Year, Month = tblPlan.Month, LineId = tblPlan.LineId, WorkingId = tblPlan.WorkingId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("WorkingPlanDetail", "WorkingPlan", new { Year = tblPlan.Year, Month = tblPlan.Month, LineId = tblPlan.LineId, WorkingId = tblPlan.WorkingId });
                }

            }
            return View(tblPlan);
        }*/
        /*public ActionResult EditOld(int Id, string Year = "", string Month = "")
        {
            WorkingPlanForm w = new WorkingPlanForm();
            int iYear = 0;
            if (!string.IsNullOrEmpty(Year))
            {
                iYear = Convert.ToInt32(Year);
            }
            int iMonth = 0;
            if (!string.IsNullOrEmpty(Month))
            {
                iMonth = Convert.ToInt32(Month);
            }
            w.Cast(iYear, iMonth, Id);

            ViewBag.Year = iYear > 0 ? iYear : w.Year;
            ViewBag.Month = iMonth > 0 ? iMonth : w.Month;
            ViewBag.WorkingId = w.WorkingId;
            ViewBag.WorkingPlanDays = "; " + w.Days;
            ViewBag.WorkingPlanShifs = "; " + w.ShiftIds;
            ViewBag.WorkingPlanLines = "; " + w.LineIds;
            ViewBag.HeadCount = w.HeadCount;
            ViewBag.Quality = w.Quality;
            ViewBag.TaktTime = w.TaktTime;
            ViewBag.Plan = w.Plan;

            ViewBag.StartHour = w.StartHour;
            ViewBag.B1StartHour = w.B1StartHour;
            ViewBag.B2StartHour = w.B2StartHour;
            ViewBag.B3StartHour = w.B3StartHour;
            ViewBag.StartMinute = w.StartMinute;
            ViewBag.B1StartMinute = w.B1StartMinute;
            ViewBag.B2StartMinute = w.B2StartMinute;
            ViewBag.B3StartMinute = w.B3StartMinute;

            ViewBag.FinishHour = w.FinishHour;
            ViewBag.B1FinishHour = w.B1FinishHour;
            ViewBag.B2FinishHour = w.B2FinishHour;
            ViewBag.B3FinishHour = w.B3FinishHour;
            ViewBag.FinishMinute = w.FinishMinute;
            ViewBag.B1FinishMinute = w.B1FinishMinute;
            ViewBag.B2FinishMinute = w.B2FinishMinute;
            ViewBag.B3FinishMinute = w.B3FinishMinute;

            List<SelectListItem> Years = new List<SelectListItem>();
            for (int i = ViewBag.Year; i <= ViewBag.Year + 1; i++)
            {
                Years.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Years = Years;

            List<SelectListItem> Months = new List<SelectListItem>();
            for (int i = 0; i <= 12; i++)
            {
                Months.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            ViewBag.Months = Months;

            ViewBag.Days = DateTime.DaysInMonth(ViewBag.Year, ViewBag.Month);
            ViewBag.Lines = new LineDao().listAll();
            ViewBag.Shifts = new WorkingShiftDao().listAll();
            List<tblBreakTime> _BreakTimes = new BreakTimeDao().listAll();

            ViewBag.BreakTimes = _BreakTimes;
            //ViewBag.NodeTypes = new NodeTypeDao().listAll(true);

            return View();
        }*/
        public ActionResult Delete(string Id, string WorkOrderId, string Quantity, string WorkPlanId)
        {
            int iId = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                iId = Convert.ToInt32(Id);
            }
            int iQuantity = 0;
            if (!string.IsNullOrEmpty(Quantity))
            {
                iQuantity = Convert.ToInt32(Quantity);
            }
            int iWorkOrderId = 0;
            if (!string.IsNullOrEmpty(WorkOrderId))
            {
                iWorkOrderId = Convert.ToInt32(WorkOrderId);
            }

            int iWorkPlanId = 0;
            if (!string.IsNullOrEmpty(WorkPlanId))
            {
                iWorkPlanId = Convert.ToInt32(WorkPlanId);
            }
            if (iId != 0)
            {

                WorkOrderPlanDao dao = new WorkOrderPlanDao();
                dao.Delete(iWorkOrderId, iWorkPlanId);
                //update workorder
                WorkOrderDao daoorder = new WorkOrderDao();
                daoorder.UpdateQuantity(iWorkOrderId, iQuantity, DateTime.Now, DateTime.Now, false, false);
            }
            return RedirectToAction("WorkingPlanDetail", "WorkingPlan", new { Id = WorkPlanId });
        }

        /*public bool checkIsLineProduction(int WorkingId, int LineId)
        {
            tblLineProduction lineProduct = new LineProducttionDao().getOEEInProduction(LineId, WorkingId);
            if (lineProduct != null && lineProduct.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }*/
        public bool checkWorkingPlan(string LineId, int Year, int Month, int Day, int StartHour, int StartMinute, int FinishHour, int FinishMinute)
        {
            int iCount = 0;
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            using (SqlConnection con = new SqlConnection(strConStr))
            {
                con.Open();
                string strQuery = "exec [CheckConflictWorkingPlan] @LineId=" + iLineId +
                    ",@Year = " + Year + ", @Month = " + Month + ", @Day = " + Day +
                    ",@StartHour=" + StartHour + ",@StartMinute=" + StartMinute + ",@FinishHour=" + FinishHour + ",@FinishMinute=" + FinishMinute;

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    iCount++;
                }
            }
            return iCount == 0 ? true : false;
        }
        public int caculaterTime(int StartHour, int StartMinute, int FinishHour, int FinishMinute)
        {
            DateTime StartTime = new DateTime(2019, 1, 1, StartHour, StartMinute, 0);
            DateTime FinishTime = new DateTime(2019, 1, 1, FinishHour, FinishMinute, 0);
            if (FinishHour < StartHour)
            {
                FinishTime = new DateTime(2019, 1, 2, FinishHour, FinishMinute, 0);
            }

            return (int)(FinishTime - StartTime).TotalMinutes;
        }
        [WebMethod]
        public JsonResult InserOrUpdateWorkingPlan(
            string Id, string Year, string Month, string Shifts, string Days, string Lines, string Status, string EmployeeId, string HeadCount, string Priority,
            string StartHour, string B1StartHour, string B2StartHour, string B3StartHour, string StartMinute, string B1StartMinute, string B2StartMinute, string B3StartMinute,
            string FinishHour, string B1FinishHour, string B2FinishHour, string B3FinishHour, string FinishMinute, string B1FinishMinute, string B2FinishMinute, string B3FinishMinute)
        {
            int _Year = int.Parse(Year);
            int _Month = int.Parse(Month);

            //Chèn thêm mới vào
            string[] arrShift = Shifts.Split(';');
            string[] arrDay = Days.Split(';');
            string[] arrLine = Lines.Split(';');

            int _StartHour = 0, _B1StartHour = 0, _B2StartHour = 0, _B3StartHour = 0, _StartMinute = 0, _B1StartMinute = 0, _B2StartMinute = 0, _B3StartMinute = 0;
            int _FinishHour = 0, _B1FinishHour = 0, _B2FinishHour = 0, _B3FinishHour = 0, _FinishMinute = 0, _B1FinishMinute = 0, _B2FinishMinute = 0, _B3FinishMinute = 0;
            if (!string.IsNullOrEmpty(StartHour)) { _StartHour = int.Parse(StartHour); }
            if (!string.IsNullOrEmpty(B1StartHour)) { _B1StartHour = int.Parse(B1StartHour); }
            if (!string.IsNullOrEmpty(B2StartHour)) { _B2StartHour = int.Parse(B2StartHour); }
            if (!string.IsNullOrEmpty(B3StartHour)) { _B3StartHour = int.Parse(B3StartHour); }
            if (!string.IsNullOrEmpty(StartMinute)) { _StartMinute = int.Parse(StartMinute); }
            if (!string.IsNullOrEmpty(B1StartMinute)) { _B1StartMinute = int.Parse(B1StartMinute); }
            if (!string.IsNullOrEmpty(B2StartMinute)) { _B2StartMinute = int.Parse(B2StartMinute); }
            if (!string.IsNullOrEmpty(B3StartMinute)) { _B3StartMinute = int.Parse(B3StartMinute); }

            if (!string.IsNullOrEmpty(FinishHour)) { _FinishHour = int.Parse(FinishHour); }
            if (!string.IsNullOrEmpty(B1FinishHour)) { _B1FinishHour = int.Parse(B1FinishHour); }
            if (!string.IsNullOrEmpty(B2FinishHour)) { _B2FinishHour = int.Parse(B2FinishHour); }
            if (!string.IsNullOrEmpty(B3FinishHour)) { _B3FinishHour = int.Parse(B3FinishHour); }
            if (!string.IsNullOrEmpty(FinishMinute)) { _FinishMinute = int.Parse(FinishMinute); }
            if (!string.IsNullOrEmpty(B1FinishMinute)) { _B1FinishMinute = int.Parse(B1FinishMinute); }
            if (!string.IsNullOrEmpty(B2FinishMinute)) { _B2FinishMinute = int.Parse(B2FinishMinute); }
            if (!string.IsNullOrEmpty(B3FinishMinute)) { _B3FinishMinute = int.Parse(B3FinishMinute); }
            int iPriority = 0;
            int iHeadCount = 0;
            byte iStatus = 0;
            if (!string.IsNullOrEmpty(Priority)) { iPriority = int.Parse(Priority); }
            if (!string.IsNullOrEmpty(HeadCount)) { iPriority = int.Parse(HeadCount); }
            if (!string.IsNullOrEmpty(Status)) { iStatus = byte.Parse(Status); }

            WorkPlanDao dao = new WorkPlanDao();
            foreach (string _shift in arrShift)
            {
                if (_shift != "")
                {

                    foreach (string _day in arrDay)
                    {
                        if (_day != "")
                        {
                            int dday = 0;
                            if (!string.IsNullOrEmpty(_day))
                            {
                                dday = Convert.ToInt32(_day);
                            }
                            else
                            {
                                continue;
                            }
                            if (dday == 0)
                            {
                                continue;
                            }

                            DateTime PlanStart = new DateTime(_Year, _Month, dday, _StartHour, _StartMinute, 0);
                            if (_StartHour >= 22)
                            {
                                dday = dday + 1;
                            }
                            DateTime PlanFinish = new DateTime(_Year, _Month, dday, _FinishHour, _FinishMinute, 0);
                            bool IB1 = false, IB2 = false, IB3 = false;
                            DateTime B1StartBreakTime = DateTime.Now,
                                B1FinishBreakTime = DateTime.Now,
                                B2StartBreakTime = DateTime.Now,
                                B2FinishBreakTime = DateTime.Now,
                                B3StartBreakTime = DateTime.Now,
                                B3FinishBreakTime = DateTime.Now;
                            if (_B1StartHour > 0 && _B1StartMinute > 0 && _B1FinishHour > 0 && _B1FinishMinute > 0)
                            {
                                B1StartBreakTime = new DateTime(_Year, _Month, dday, _B1StartHour, _B1StartMinute, 0);
                                B1FinishBreakTime = new DateTime(_Year, _Month, dday, _B1FinishHour, _B1FinishMinute, 0);
                                IB1 = true;
                            }
                            if (_B2StartHour > 0 && _B2StartMinute > 0 && _B2FinishHour > 0 && _B2FinishMinute > 0)
                            {
                                B2StartBreakTime = new DateTime(_Year, _Month, dday, _B2StartHour, _B2StartMinute, 0);
                                B2FinishBreakTime = new DateTime(_Year, _Month, dday, _B2FinishHour, _B2FinishMinute, 0);
                                IB2 = true;
                            }
                            if (_B3StartHour > 0 && _B3StartMinute > 0 && _B3FinishHour > 0 && _B3FinishMinute > 0)
                            {
                                B3StartBreakTime = new DateTime(_Year, _Month, dday, _B3StartHour, _B3StartMinute, 0);
                                B3FinishBreakTime = new DateTime(_Year, _Month, dday, _B3FinishHour, _B3FinishMinute, 0);
                                IB3 = true;
                            }
                            foreach (string _line in arrLine)
                            {
                                if (!string.IsNullOrEmpty(_line))
                                {
                                    tblWorkPlan plan = new tblWorkPlan();
                                    plan.LineId = Convert.ToInt32(_line);
                                    plan.PlanStart = PlanStart;
                                    plan.PlanFinish = PlanFinish;
                                    plan.PlanHeadCount = iHeadCount;
                                    plan.Priority = iPriority;
                                    plan.Status = iStatus;
                                    //plan.EmployeeId = EmployeeId;
                                    if (IB1)
                                    {
                                        //plan.PlanBreakStart1 = B1StartBreakTime;
                                        //plan.PlanBreakFinish1 = B1FinishBreakTime;
                                    }
                                    if (IB2)
                                    {
                                        //plan.PlanBreakStart2 = B2StartBreakTime;
                                        //plan.PlanBreakFinish2 = B2FinishBreakTime;
                                    }
                                    if (IB3)
                                    {
                                        //plan.PlanBreakStart3 = B3StartBreakTime;
                                        //plan.PlanBreakFinish3 = B3FinishBreakTime;
                                    }
                                    dao.Insert(plan);
                                }
                            }

                        }

                    }
                }
                //}

                ////Chạy lại phần báo cáo

                // using (SqlConnection con = new SqlConnection(strConStr))
                //{

                // con.Open();

            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetWorkOrder(string Code)
        {
            List<WorkOrderForm> lstWOF = new List<WorkOrderForm>();
            WorkOrderDao dao = new WorkOrderDao();

            List<tblWorkOrder> lst = dao.listAllByWorkOrderPlan(Code);
            foreach (tblWorkOrder wO in lst)
            {
                WorkOrderForm f = new WorkOrderForm();
                f.Cast(wO);
                lstWOF.Add(f);
            }
            return Json(lstWOF, JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        public JsonResult GetWorkOrderPlan(int Id)
        {
            tblWorkOrderPlan woP = new WorkOrderPlanDao().ViewDetail(Id);
            WorkOrderPlanForm f = new WorkOrderPlanForm();
            f.Cast(woP);
            return Json(f, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertWorkOrderPlan(string Id, string WorkOrderId, string WorkPlanId,
            string Quantity, string StartTime, string FinishTime, string nOrder, string TaktTime, string Action)
        {
            tblWorkOrderPlan wop = new tblWorkOrderPlan();
            WorkOrderPlanDao dao = new WorkOrderPlanDao();
            bool isInsert = true;
            int iAction = 1;
            int QuantityOld = 0;
            if (!string.IsNullOrEmpty(Action))
            {
                try
                {
                    iAction = Convert.ToInt32(Action);
                }
                catch
                {
                    isInsert = false;
                }
            }
            if (iAction == 2)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    try
                    {
                        wop = dao.ViewDetail(Convert.ToInt32(Id));
                        if (wop.PlanQuantity != null)
                        {
                            QuantityOld = Convert.ToInt32(wop.PlanQuantity);
                        }
                    }
                    catch
                    {
                        isInsert = false;
                    }
                }
            }
            if (!string.IsNullOrEmpty(WorkOrderId))
            {
                try
                {
                    // wop.WorkOrderId = Convert.ToInt32(WorkOrderId);
                    tblWorkOrder _workOrder = new WorkOrderDao().ViewDetailForWorkOrderCode(wop.WorkOrderCode);
                    wop.WorkOrderCode = _workOrder.Code;
                    tblProduct _p = new ProductDao().ViewDetail(_workOrder.ProductCode);
                    wop.ProductCode = _p.Code;
                    wop.ProductName = _p.Name;
                    wop.TaktTime = _p.TaktTime;
                    //wop.ProductCode
                }
                catch
                {
                    isInsert = false;
                }
            }
            if (!string.IsNullOrEmpty(WorkPlanId))
            {
                try
                {
                    wop.WorkPlanId = Convert.ToInt32(WorkPlanId);
                }
                catch
                {
                    isInsert = false;
                }
            }
            int iQuantity = 0;
            if (!string.IsNullOrEmpty(Quantity))
            {
                try
                {
                    wop.PlanQuantity = iQuantity = Convert.ToInt32(Quantity);
                }
                catch
                {
                    isInsert = false;
                }
            }
            bool isUpdateStartFinish = true;
            DateTime StartTimeUpdate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartTime))
            {
                try
                {
                    DateTime oDate = DateTime.ParseExact(StartTime, "yyyy/MM/dd HH:mm:ss", null);
                    wop.PlanStart = StartTimeUpdate = oDate;
                }
                catch
                {
                    isUpdateStartFinish = false;
                }
            }
            DateTime FinishTimeUpdate = DateTime.Now;
            if (!string.IsNullOrEmpty(FinishTime))
            {
                try
                {
                    DateTime oDate = DateTime.ParseExact(FinishTime, "yyyy/MM/dd HH:mm:ss", null);
                    wop.PlanFinish = FinishTimeUpdate = oDate;
                }
                catch
                {
                    isUpdateStartFinish = false;
                }
            }
            if (!string.IsNullOrEmpty(nOrder))
            {
                try
                {
                    wop.nOrder = Convert.ToInt32(nOrder);
                }
                catch
                {

                }
            }
            if (isInsert)
            {

                if (iAction == 1)
                {
                    dao.Insert(wop);
                    //update WorkOrder
                    WorkOrderDao daoWO = new WorkOrderDao();
                    daoWO.UpdateQuantity(daoWO.ViewDetailForWorkOrderCode(wop.WorkOrderCode).Id, iQuantity, StartTimeUpdate, FinishTimeUpdate, isUpdateStartFinish, true);
                }
                else if (iAction == 2)
                {
                    dao.Update(wop);
                    WorkOrderDao daoWO = new WorkOrderDao();
                    bool isAdd = true;
                    int iiQuantity = 0;
                    if (QuantityOld < iQuantity)
                    {
                        iiQuantity = iQuantity - QuantityOld;
                    }
                    else
                    {
                        isAdd = false;
                        iiQuantity = QuantityOld - iQuantity;
                    }

                    daoWO.UpdateQuantity(daoWO.ViewDetailForWorkOrderCode(wop.WorkOrderCode).Id, iiQuantity, StartTimeUpdate, FinishTimeUpdate, isUpdateStartFinish, isAdd);
                }
            }
            return RedirectToAction("WorkingPlanDetail", "WorkingPlan", new { Id = WorkPlanId });
        }



    }
}