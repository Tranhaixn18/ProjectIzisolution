using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using Common;
using Model.Dao;
using Model.DataModel;
using avSVAW.Models;
using System.Configuration;
using ClosedXML.Excel;
using System.IO;
using System.Data.SqlClient;
using Model.Models;

namespace avSVAW.Controllers
{
    public class ReportController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        int Day2Close = int.Parse(ConfigurationManager.AppSettings["Day2Close"]);
        int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);
        string ConfigPath = ConfigurationManager.AppSettings["ConfigPath"].ToString();

       /* public ActionResult ReportWorkOrderPlan(string StartDate, string EndDate, string LineId, string NodeId, string Product)
        {
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            ViewBag.LineId = LineId;
            List<SelectListItem> Lines = new List<SelectListItem>();
            List<tblLine> lst = new LineDao().listAll();
            foreach (var nt in lst)
            {
                Lines.Add(new SelectListItem() { Value = nt.Id.ToString(), Text = nt.Name.ToString() });
            }

            Lines.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "" });

            ViewBag.Lines = Lines;

            int iNode = 0;
            GetNode(NodeId, out iNode);
            string VProduct = "";
            if (!string.IsNullOrEmpty(Product))
            {
                try
                {
                    VProduct = Product;
                }
                catch { }
            }
            ViewBag.Product = Product;

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    var FromDate1 = Convert.ToDateTime(StartDate.Trim());
                    FromDate = new DateTime(FromDate1.Year, FromDate1.Month, FromDate1.Day, Hour2UpdateReportDaily, 0, 0);
                }
                catch { }
            }
            ViewBag.StartDate = FromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    var ToDate1 = Convert.ToDateTime(EndDate.Trim());
                    ToDate = new DateTime(ToDate1.Year, ToDate1.Month, ToDate1.Day + 1, Hour2UpdateReportDaily, 0, 0);
                }
                catch { }
            }
            ViewBag.EndDate = ToDate.ToString("yyyy/MM/dd");
            List<tblWorkPlan> lstWorkPlans = new List<tblWorkPlan>();
            List<WorkPlanForm> WpF = new List<WorkPlanForm>();
            lstWorkPlans = new WorkPlanDao().ListAllByFilterReport(iLineId, FromDate, ToDate);
            int[] wplans = new int[lstWorkPlans.Count];
            int i = 0;
            foreach (tblWorkPlan wp in lstWorkPlans)
            {
                WorkPlanForm fff = new WorkPlanForm();
                fff.Cast(wp);
                WpF.Add(fff);
                wplans[i] = wp.Id;
                i++;
            }
            ViewBag.WorkPlans = WpF;
            //tblWorkOrderPlan t = new tblWorkOrderPlan();t.WorkPlanId
            Product = Product == null ? "" : Product;
            List<tblWorkOrderPlan> lstWOP = new WorkOrderPlanDao().getByFilter(wplans, iNode, Product.Trim());
            List<WorkOrderPlanForm> models = new List<WorkOrderPlanForm>();
            foreach (tblWorkOrderPlan wo in lstWOP)
            {
                WorkOrderPlanForm f = new WorkOrderPlanForm();
                f.Cast(wo);
                models.Add(f);
            }
            return View("ReportWorkOrderPlan", models);
        }*/
        public ActionResult ReportWorkingPlanDetail(string Id, string LineId, string NodeId)
        {
            int iId = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                iId = Convert.ToInt32(Id);
            }

            ViewBag.WorkPlanId = iId;

            ViewBag.LineId = LineId;
            ViewBag.NodeId = NodeId;

            tblWorkPlan workPlan = new WorkPlanDao().ViewDetail(iId);
            WorkPlanForm planForm = new WorkPlanForm();
            planForm.Cast(workPlan);
            ViewBag.WorkPlanForm = planForm;
            List<tblWorkOrderPlan> lstWOP = new WorkOrderPlanDao().listAllByFilter(iId);
            List<WorkOrderPlanForm> model = new List<WorkOrderPlanForm>();
            foreach (tblWorkOrderPlan wop in lstWOP)
            {
                WorkOrderPlanForm wopf = new WorkOrderPlanForm();
                wopf.Cast(wop);
                model.Add(wopf);
            }
            return View("ReportWorkingPlanDetail", model);
        }
        public ActionResult OperationNodeReport(string StartDate, string EndDate, string LineId, string NodeId)
        {
            List<tblEventReason> lstReasons = new EventReasonDao().listAll();

            //  var groupReason = lstReasons.GroupBy(item => item.GroupName).Select(group => new GroupReasonForm { GroupName = group.Key, Items = group.ToList() }).ToList();
            ViewBag.Reasons = lstReasons;

            //load workshift
            int iLineId = 0;
            GetNodeLine(LineId, out iLineId);
            int iNode = 0;
            GetNode(NodeId, out iNode);

            List<tblEventDef> EventDefs = new EventDefDao().listAll();
            ViewBag.EventDefs = EventDefs;
            ViewBag.LineId = LineId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(LineId))
            {
                try
                {
                    iLineId = Convert.ToInt32(LineId);
                }
                catch { }
            }

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }
            ViewBag.StartDate = FromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }
            ViewBag.EndDate = ToDate.ToString("yyyy/MM/dd");

            return View("OperationNodeReport");
        }
        /*[WebMethod]*/
        /*public JsonResult GetOperationNodeReport(string StartDate, string EndDate, string LineId, string NodeId)
        {
            int iLineId = 0;
            GetNodeLine(LineId, out iLineId);
            int iNode = 0;
            GetNode(NodeId, out iNode);
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }
            List<tblNodeEvent> lstNodeEvent = new NodeEventDao().listByFilterForTimeLine(iLineId, iNode,
                FromDate.Year, FromDate.Month, FromDate.Day, Hour2UpdateReportDaily, 0,
                ToDate.Year, ToDate.Month, ToDate.Day, Hour2UpdateReportDaily, 0
                );

            foreach (tblNodeEvent n in lstNodeEvent)
            {
                NodeEventForm f = new NodeEventForm();
                f.Cast(n);

            }

            List<NodeEventForm> model = new List<NodeEventForm>();
            using (SqlConnection con = new SqlConnection(strConStr))
            {

                con.Open();

                string strQuery = "exec [ViewOperationEvent] @StartDate = '" + FromDate.ToString("yyyy-MM-dd") +
                    "', @EndDate = '" + ToDate.ToString("yyyy-MM-dd") + "', @LineId = " + iLineId +
                    ", @NodeId = " + iNode + ", @Hour2Update = " + Hour2UpdateReportDaily;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    NodeEventForm f = new NodeEventForm();
                    f.Reason = dr["Reason"] == null ? "" : dr["Reason"].ToString();
                    f.NodeId = dr["NodeId"] == null ? 0 : int.Parse(dr["NodeId"].ToString());
                    
                    f.Id = dr["Id"] == null ? 0 : int.Parse(dr["Id"].ToString());
                    f.EventDefId = dr["EventDefId"] == null ? 0 : int.Parse(dr["EventDefId"].ToString());
                    f.strStartDate = dr["T3"] == null ? "" : Convert.ToDateTime(dr["T3"]).ToString("yyyy/MM/dd HH:mm:ss");
                    f.strFromDate = dr["T1"] == null ? "" : Convert.ToDateTime(dr["T1"]).ToString("yyyy/MM/dd HH:mm:ss");
                    f.NodeName = dr["NodeName"] == null ? "" : dr["NodeName"].ToString();
                    model.Add(f);
                    //form.LineId = int.Parse(dr["LineId"].ToString());
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }*/
        /*[HttpGet]*/
        /*public FileContentResult ExportListLossToExcel(string StartDate, string EndDate, string LineId)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime FromDate = DateTime.Today;

            DateTime ToDate = DateTime.Today; //WebConstants.avMaxValue;

            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }
            int iLineId = 0;

            if (!string.IsNullOrEmpty(LineId))
            {
                try
                {
                    iLineId = Convert.ToInt32(LineId.Trim());
                }
                catch { }
            }

            string strFullpath = ConfigPath;
            string strTemplateFile = strFullpath + @"\Reports\Templates\ReportCallLoss.xlsx";
            string strFileName = strFullpath;
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Report1");

            ws.Cell("C4").Value = "'" + FromDate.ToString("dd/MM/yyyy");
            ws.Cell("E4").Value = "'" + ToDate.ToString("dd/MM/yyyy");
            string Linestr = Resources.Language.All;
            int StartRow = 6, iCount = 1;
            List<tblNodeEvent> lstNEvent = new NodeEventDao().listByDate(
                FromDate.Year, FromDate.Month, FromDate.Day, 0, 1,
                ToDate.Year, ToDate.Month, ToDate.Day, 23, 59
                );
            foreach (tblNodeEvent nE in lstNEvent)
            {
                ListLossForm llF = new ListLossForm();
                llF.Cast(nE);
                if (UserFunction.USER_LINE_LEADER(session.Role))
                {
                    if (session.LineId == llF.LineId)
                    {
                        ws.Cell("A" + (StartRow + iCount)).Value = iCount;
                        //ws.Cell("B" + (StartRow + iCount)).Value = _row.ReceiptCode;
                        ws.Cell("B" + (StartRow + iCount)).Value = llF.LineName;
                        Linestr = llF.LineName;
                        ws.Cell("C" + (StartRow + iCount)).Value = llF.NodeName;
                        ws.Cell("D" + (StartRow + iCount)).Value = llF.ProblemEN;
                        ws.Cell("E" + (StartRow + iCount)).Value = llF.Reason;
                        ws.Cell("F" + (StartRow + iCount)).Value = "'" + llF.TT3;
                        ws.Cell("G" + (StartRow + iCount)).Value = "'" + llF.TotalDuration;
                        if (iCount < lstNEvent.Count)
                        {
                            ws.Row(StartRow + iCount).InsertRowsBelow(1);
                            iCount++;
                        }
                    }
                }
                else
                {
                    if (iLineId == 0 || llF.LineId == iLineId)
                    {
                        ws.Cell("A" + (StartRow + iCount)).Value = iCount;
                        //ws.Cell("B" + (StartRow + iCount)).Value = _row.ReceiptCode;
                        if (iLineId > 0)
                        {
                            Linestr = llF.LineName;
                        }
                        ws.Cell("B" + (StartRow + iCount)).Value = llF.LineName;
                        ws.Cell("C" + (StartRow + iCount)).Value = llF.NodeName;
                        ws.Cell("D" + (StartRow + iCount)).Value = llF.ProblemEN;
                        ws.Cell("E" + (StartRow + iCount)).Value = llF.Reason;
                        ws.Cell("F" + (StartRow + iCount)).Value = "'" + llF.TT3;
                        ws.Cell("G" + (StartRow + iCount)).Value = "'" + llF.TotalDuration;

                        if (iCount < lstNEvent.Count)
                        {
                            ws.Row(StartRow + iCount).InsertRowsBelow(1);
                            iCount++;
                        }
                    }

                }
            }
            ws.Cell("G4").Value = Linestr;
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportReason_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".xlsx");
            }
        }*/
        /*[HttpGet]*/
        /*public FileContentResult OperationNodeToExcel(string StartDate, string EndDate, string LineId, string NodeId)
        {
            int iNode = 0;
            GetNode(NodeId, out iNode);

            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }
            ViewBag.StartDate = FromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }

            List<tblNodeEvent> lstNodeEvent = new NodeEventDao().listByFilter(iLineId, iNode,
              FromDate.Year, FromDate.Month, FromDate.Day, Hour2UpdateReportDaily, 0,
              ToDate.Year, ToDate.Month, ToDate.Day, Hour2UpdateReportDaily, 0
              );

            int iCount = 0;

            // huantn 2019-09-05 
            string strFullpath = ConfigPath;
            string strTemplateFile = strFullpath + @"\Reports\Templates\OperationNodeReport_Template.xlsx";
            string fileName = "OperationNodeReport_";


            XLWorkbook wb = new XLWorkbook(strTemplateFile);

            //Lấy thằng worksheet
            IXLWorksheet ws = wb.Worksheet("F2EFF");
            //Điền mấy thằng râu ria
            string lineName = "Tất cả";
            if (iLineId > 0)
            {
                tblLine ll = new LineDao().ViewDetail(iLineId);
                lineName = ll.Name;
            }
            ws.Cell("D4").Value = "'" + lineName;
            ws.Cell("F4").Value = "'" + reportDate.ToString("dd/MM/yyyy");

            int StartRow = 7, iTotal = 0;

            foreach (tblNodeEvent n in lstNodeEvent)
            {
                iTotal++;
                OperationForm _form = new OperationForm();

                // _form = GetEachNodeEvent(dr);
                string LineName = "";
                string NodeName = "";
                string EventDefName = "";
                if (n.NodeId != null)
                {
                    tblNode node = new NodeDao().ViewDetail(Convert.ToInt32(n.NodeId));
                    NodeName = node.Name;
                    LineName = new LineDao().ViewDetail(Convert.ToInt32(node.LineId)).Name;
                }
                if (n.EventDefId != null)
                {
                    EventDefName = new EventDefDao().ViewDetail(Convert.ToInt32(n.EventDefId)).Name_VN;
                }
                string strT3 = "";
                if (n.T3 != null)
                {
                    strT3 = Convert.ToDateTime(n.T3).ToString("yyyy/MM/dd HH:mm:ss");

                }
                string strDuration = "";
                string strT1 = "";
                if (n.T1 != null)
                {
                    var iven = Convert.ToDateTime(n.T1) - Convert.ToDateTime(n.T3);
                    strT1 = Convert.ToDateTime(n.T1).ToString("yyyy/MM/dd HH:mm:ss");
                    strDuration = ConvertMin2Time(iven.TotalMinutes);
                }
                ws.Cell("A" + (StartRow + iCount)).Value = (iCount + 1);
                ws.Cell("B" + (StartRow + iCount)).Value = LineName;
                ws.Cell("C" + (StartRow + iCount)).Value = "'" + NodeName;
                ws.Cell("D" + (StartRow + iCount)).Value = EventDefName;
                ws.Cell("E" + (StartRow + iCount)).Value = "'" + strT3;
                ws.Cell("F" + (StartRow + iCount)).Value = "'" + strT1;
                ws.Cell("G" + (StartRow + iCount)).Value = strDuration;

                if (lstNodeEvent.Count > iTotal)
                {
                    ws.Row(StartRow + iCount).InsertRowsBelow(1);
                }
                iCount++;

            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".xlsx");
            }
        }*/

        /*public JsonResult GetChartProduction(string StrDate, string LineId)
        {

            DateTime DDate = DateTime.Now;

            if (!string.IsNullOrEmpty(StrDate))
            {
                try
                {
                    DDate = Convert.ToDateTime(StrDate.Trim());
                }
                catch { }
            }

            int iLineId = 0;

            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (UserFunction.USER_LINE_LEADER(session.Role))
            {
                iLineId = session.LineId == null ? 0 : Convert.ToInt32(session.LineId);
            }
            ViewBag.LineId = iLineId;

            // int NumberOfDays = (int)(endDate - startDate).TotalDays;

            List<tblWorkPlan> lstWorkingPlan = new WorkingPlanDao().ListByDate(DDate, iLineId).ToList();
            List<ProductionForm> lst = new List<ProductionForm>();
            if (lstWorkingPlan.Count > 0)
            {
                foreach (tblWorkPlan l in lstWorkingPlan)
                {
                    ProductionForm productionForm = new ProductionForm();
                    List<tblPMSCache> pmsCache = new PMSCacheDao().listByLineAndWorkingId(iLineId, l.Id);
                    List<pmscaches> pms = new List<pmscaches>();
                    if (pmsCache.Count > 0)
                    {
                        for (int i = 0; i < pmsCache.Count(); i++)
                        {
                            pmscaches p = new pmscaches();
                            p.ActualQuantity = pmsCache[i].ActualQuantity;
                            p.Id = pmsCache[i].Id;
                            p.LineId = pmsCache[i].LineId;
                            p.LogsTime = pmsCache[i].LogsTime;
                            p.PlanDate = pmsCache[i].PlanDate;
                            p.PlanId = pmsCache[i].PlanId;
                            p.PlanQuantity = pmsCache[i].PlanQuantity;
                            p.PONumber = pmsCache[i].PONumber;
                            p.ProductCode = pmsCache[i].ProductCode;
                            p.ProductModel = pmsCache[i].ProductModel;
                            p.ProductName = pmsCache[i].ProductName;
                            p.StartAt = pmsCache[i].StartAt;
                            p.WorkingPlanId = pmsCache[i].WorkingPlanId;
                            p.cast();
                            pms.Add(p);
                        }
                        productionForm.pmsCache = pms;

                        productionForm.StartHour = l.StartHour.ToString("00");
                        productionForm.StartMinute = l.StartMinute.ToString("00");
                        productionForm.FinishHour = l.FinishHour.ToString("00");
                        productionForm.FinishMinute = l.FinishMinute.ToString("00");
                        lst.Add(productionForm);
                    }
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }*/

        public ActionResult ChartProduction(string StartDate, string EndDate, string ProductCode, string TypeChoose)
        {
            //Kiểm tra điều kiện tìm kiếm

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }
            ViewBag.StartDate = FromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }
            ViewBag.EndDate = ToDate.ToString("yyyy/MM/dd");
            ViewBag.ProductCode = ProductCode;
            ViewBag.TypeChoose = TypeChoose;
            List<ProductChartForm> models = new List<ProductChartForm>();

            using (SqlConnection con = new SqlConnection(strConStr))
            {

                con.Open();
                string strQuery = "exec [SP_GetProductionReport] @StartYear = '" + FromDate.Year +
                     "', @StartMonth = '" + FromDate.Month +
                     "', @StartDay = '" + FromDate.Day +
                     "', @EndYear = '" + ToDate.Year +
                     "', @EndMonth = '" + ToDate.Month +
                     "', @EndDay = '" + ToDate.Day +
                     "', @ProductCode = '" + ProductCode + "'";
                ;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProductChartForm c = new ProductChartForm();
                    c.Date = dr["Date"] == DBNull.Value ? "" : dr["Date"].ToString();
                    c.Quantity = dr["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Quantity"].ToString());
                    c.TaktTime = dr["TaktTime"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TaktTime"].ToString()); ;
                    c.UPH = dr["UPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UPH"].ToString()); ;
                    c.UPPH = dr["UPPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UPPH"].ToString()); ;
                    models.Add(c);
                    // form.LineId = int.Parse(dr["LineId"].ToString());
                }
            }

            return View("ChartProduction", models);
        }
        /*public ActionResult GetCallLog(string StartDate, string EndDate, string LineId)
        {
            List<tblEventReason> lstReasons = new EventReasonDao().listAll();

            //  var groupReason = lstReasons.GroupBy(item => item.GroupName).Select(group => new GroupReasonForm { GroupName = group.Key, Items = group.ToList() }).ToList();
            ViewBag.Reasons = lstReasons;

            int iLineId = 0;
            GetLines(LineId, out iLineId);
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(StartDate))
            {
                try
                {
                    FromDate = Convert.ToDateTime(StartDate.Trim());
                }
                catch { }
            }
            ViewBag.StartDate = FromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(EndDate))
            {
                try
                {
                    ToDate = Convert.ToDateTime(EndDate.Trim());
                }
                catch { }
            }
            ViewBag.EndDate = ToDate.ToString("yyyy/MM/dd");
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];

            List<tblNodeEvent> lstNEvent = new NodeEventDao().listByFilter(iLineId, 0,
                FromDate.Year, FromDate.Month, FromDate.Day, Hour2UpdateReportDaily, 0,
                ToDate.Year, ToDate.Month, ToDate.Day, Hour2UpdateReportDaily, 0
                );
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
                    if (iLineId == 0 || llF.LineId == iLineId)
                    {
                        models.Add(llF);
                    }

                }
            }
            List<tblEventDef> lstEventD = new EventDefDao().listAll();
            var chart = models.GroupBy(x => x.EventDefId).Select(g => new ListLossForm { EventDefId = g.Key, LineId = g.Count() });
            ViewBag.Chart = chart;
            ViewBag.lstEventD = lstEventD;
            return View("ReportCallLog", models);
        }*/


        #region function =========
        public void GetNodeLine(string NodeLine, out int iNodeLine)
        {
            iNodeLine = 0;
            if (!string.IsNullOrEmpty(NodeLine))
            {
                iNodeLine = Convert.ToInt32(NodeLine);
            }
            ViewBag.LineId = NodeLine;
            List<SelectListItem> Lines = new List<SelectListItem>();
            List<tblLine> lst = new LineDao().listAll();
            foreach (var nt in lst)
            {
                Lines.Add(new SelectListItem() { Value = nt.Id.ToString(), Text = nt.Name.ToString() });
            }

            Lines.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "" });

            ViewBag.Lines = Lines;
        }
        public void GetProduct(string Product, out int iProduct)
        {
            iProduct = 0;
            if (!string.IsNullOrEmpty(Product))
            {
                iProduct = Convert.ToInt32(Product);
            }
            ViewBag.ProductId = Product;
            List<SelectListItem> products = new List<SelectListItem>();
            List<tblProduct> listPro = new ProductDao().ListAll();
            foreach (var item in listPro)
            {
                products.Add(new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            products.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.Products = products;
        }
        public void GetNodeSecond(string Node, out int iNode)
        {
            iNode = 0;
            if (!string.IsNullOrEmpty(Node))
            {
                iNode = Convert.ToInt32(Node);
            }
            ViewBag.NodeId = Node;
            List<SelectListItem> nodes = new List<SelectListItem>();
            IEnumerable<tblNode> tblNodes = new NodeDao().ListAll();
            foreach (var item in tblNodes)
            {
                nodes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            nodes.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.Nodes = nodes;
        }
        [HttpGet]
        public ActionResult GetNodeFromLineId(int lineId)
        {
            List<SelectListItem> nodes = new List<SelectListItem>();
            List<tblNode> node = new NodeDao().ListtblNodeByLineId(lineId);
            foreach (var item in node)
            {
                nodes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            nodes.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.NodesFromLine = nodes;
            return View();
        }
        public void GetNodeFromLineIdOne(string Line, out int iLineId)
        {
            iLineId = 0;
            if (!string.IsNullOrEmpty(Line))
            {
                iLineId = Convert.ToInt32(iLineId);
            }
            List<SelectListItem> nodes = new List<SelectListItem>();
            List<tblNode> node = new NodeDao().ListtblNodeByLineId(iLineId);
            foreach (var item in node)
            {
                nodes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            nodes.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.NodesFromLine = nodes;
        }
        /*public OEEForm getOEEProduction(tblLineProduction lineProduct)
        {
            OEEForm form = new OEEForm();
            form.Plan = lineProduct.Plan == 0 ? 0 : Convert.ToInt32(lineProduct.Plan);
            form.Target = lineProduct.Target == 0 ? 0 : Convert.ToInt32(lineProduct.Target);
            form.Actual = lineProduct.Actual == 0 ? 0 : Convert.ToInt32(lineProduct.Actual);
            form.NG = lineProduct.NG == 0 ? 0 : Convert.ToInt32(lineProduct.NG);

            form.StopDuration = lineProduct.StopDuration == 0 ? 0 : Convert.ToInt32(lineProduct.StopDuration);
            form.PlanDuration = lineProduct.PlanDuration == 0 ? 0 : Convert.ToInt32(lineProduct.PlanDuration);

            form.NumberOfStop = lineProduct.NumberOfStop == null ? 0 : Convert.ToInt32(lineProduct.NumberOfStop);
            form.TakeTime = lineProduct.TakeTime == null ? 0 : Convert.ToInt32(lineProduct.TakeTime);
            form.HeadCount = lineProduct.HeadCount == null ? 0 : Convert.ToInt32(lineProduct.HeadCount);
            form.StartTime = lineProduct.StartTime == null ? "" : Convert.ToDateTime(lineProduct.StartTime).ToString("HH:mm");
            form.StopTime = lineProduct.StopTime == null ? "" : Convert.ToDateTime(lineProduct.StopTime).ToString("HH:mm");


            double PlanDuration = Math.Round((double)form.PlanDuration / 60);

            form.PlanDuration = int.Parse(PlanDuration.ToString());
            double stopDura = Math.Round((double)form.StopDuration / 60);
            form.StopDuration = int.Parse(stopDura.ToString());
            form.RunningDuration = form.PlanDuration - form.StopDuration;

            double Avaibility = 0;
            double totalOEE = 1;
            int is_NotCal1 = 0;
            int is_NotCal2 = 0;
            int is_NotCal3 = 0;
            if (form.PlanDuration > 0)
            {
                is_NotCal1 = 1;
                Avaibility = Math.Round(form.RunningDuration / (double)form.PlanDuration, 1) * 100;

                totalOEE = totalOEE * Math.Round(form.RunningDuration / (double)form.PlanDuration, 1);
                // Avaibility = Avaibility > 100 ? 100 : Avaibility;
            }
            double Performance = 0;
            if (form.Target > 0)
            {
                is_NotCal2 = 1;
                Performance = Math.Round(form.Actual / (double)form.Target, 2) * 100;
                totalOEE = Math.Round((form.Actual + form.NG) / (double)form.Target, 1) * totalOEE;
                //  Performance = Performance > 100 ? 100 : Performance;
            }
            double Quality = 0;
            int TotalATNG = form.Actual + form.NG;
            if (TotalATNG > 0)
            {
                is_NotCal3 = 1;
                Quality = Math.Round(form.Actual / (double)TotalATNG, 1) * 100;
                totalOEE = Math.Round(form.Actual / (double)TotalATNG, 1) * totalOEE;
                // Quality = Quality > 100 ? 100 : Quality;
            }
            form.Avaibility = Avaibility;
            form.Performance = Performance;
            form.Quality = Quality;
            totalOEE = Math.Round(is_NotCal1 * is_NotCal2 * is_NotCal3 * totalOEE * 100, 1);

            form.OEE = totalOEE;
            return form;

        }*/
        public OEEForm GetOEEForm(DataRow dr)
        {
            OEEForm form = new OEEForm();
            form.Plan = int.Parse(dr["Plan"].ToString());
            form.Target = int.Parse(dr["Target"].ToString());
            form.Actual = int.Parse(dr["Actual"].ToString());
            form.NG = int.Parse(dr["NG"].ToString());
            form.RunningDuration = int.Parse(dr["RunningDuration"].ToString());
            form.StopDuration = int.Parse(dr["StopDuration"].ToString());
            form.PlanDuration = int.Parse(dr["PlanDuration"].ToString());
            double runningDura = Math.Round((double)form.RunningDuration / 60);
            form.RunningDuration = int.Parse(runningDura.ToString());
            double stopDura = Math.Round((double)form.StopDuration / 60);
            form.StopDuration = int.Parse(stopDura.ToString());
            double Avaibility = 0;
            double totalOEE = 1;
            int is_NotCal1 = 0;
            int is_NotCal2 = 0;
            int is_NotCal3 = 0;
            if (form.PlanDuration > 0)
            {
                is_NotCal1 = 1;
                Avaibility = Math.Round(form.RunningDuration / (double)form.PlanDuration, 2) * 100;

                totalOEE = totalOEE * Math.Round(form.RunningDuration / (double)form.PlanDuration, 2);
                // Avaibility = Avaibility > 100 ? 100 : Avaibility;
            }
            double Performance = 0;
            if (form.Target > 0)
            {
                is_NotCal2 = 1;
                Performance = Math.Round((form.Actual + form.NG) / (double)form.Target, 2) * 100;
                totalOEE = Math.Round((form.Actual + form.NG) / (double)form.Target, 2) * totalOEE;
                //  Performance = Performance > 100 ? 100 : Performance;
            }
            double Quality = 0;
            int TotalATNG = form.Actual + form.NG;
            if (TotalATNG > 0)
            {
                is_NotCal3 = 1;
                Quality = Math.Round(form.Actual / (double)TotalATNG, 2) * 100;
                totalOEE = Math.Round(form.Actual / (double)TotalATNG, 2) * totalOEE;
                // Quality = Quality > 100 ? 100 : Quality;
            }
            form.Avaibility = Avaibility;
            form.Performance = Performance;
            form.Quality = Quality;
            totalOEE = Math.Round(is_NotCal1 * is_NotCal2 * is_NotCal3 * totalOEE * 100, 2);

            form.OEE = totalOEE;
            return form;
        }
        public OperationForm GetEachLineEvent(DataRow dr)
        {
            OperationForm form = new OperationForm();

            form.LineId = int.Parse(dr["LineId"].ToString());
            form.LineName = dr["LineName"].ToString();
            form.EventDefId = int.Parse(dr["EventDefId"].ToString());
            form.EventDefName = dr["EventDefName_EN"].ToString();
            form.T3 = ((DateTime)dr["T3"]).ToString("yyyy-MM-dd HH:mm:ss");// (dr["T3"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T3"]); ;
            form.T1 = ((DateTime)dr["T1"]).ToString("yyyy-MM-dd HH:mm:ss"); //(dr["T1"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T1"]);
            //form.PlanDuration = double.Parse(dr["PlanDuration"] == DBNull.Value ? "0" : dr["PlanDuration"].ToString());
            form.ActualDuration = double.Parse(dr["ActualDuration"] == DBNull.Value ? "0" : dr["ActualDuration"].ToString());
            form.WaitDuration = double.Parse(dr["WaitDuration"] == DBNull.Value ? "0" : dr["WaitDuration"].ToString());
            form.ProcessDuration = double.Parse(dr["ProcessDuration"] == DBNull.Value ? "0" : dr["ProcessDuration"].ToString());

            form.strDuration = ConvertMin2Time(form.ActualDuration);

            return form;
        }
        public OperationForm GetEachNodeEvent(DataRow dr)
        {
            OperationForm form = new OperationForm();

            form.LineId = int.Parse(dr["LineId"].ToString());
            form.LineName = dr["LineName"].ToString();
            form.NodeName = dr["NodeName"].ToString();
            form.EventDefId = int.Parse(dr["EventDefId"].ToString());
            form.EventDefName = dr["EventDefName_EN"].ToString();
            form.T3 = ((DateTime)dr["T3"]).ToString("yyyy-MM-dd HH:mm:ss");// (dr["T3"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T3"]); ;
            form.T1 = ((DateTime)dr["T1"]).ToString("yyyy-MM-dd HH:mm:ss"); //(dr["T1"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T1"]);
            //form.PlanDuration = double.Parse(dr["PlanDuration"] == DBNull.Value ? "0" : dr["PlanDuration"].ToString());
            form.ActualDuration = double.Parse(dr["ActualDuration"] == DBNull.Value ? "0" : dr["ActualDuration"].ToString());
            form.WaitDuration = double.Parse(dr["WaitDuration"] == DBNull.Value ? "0" : dr["WaitDuration"].ToString());
            form.ProcessDuration = double.Parse(dr["ProcessDuration"] == DBNull.Value ? "0" : dr["ProcessDuration"].ToString());

            form.strDuration = ConvertMin2Time(form.ActualDuration);

            return form;
        }


        public OperationForm GetEachEvent(DataRow dr)
        {
            OperationForm form = new OperationForm();

            form.NodeId = int.Parse(dr["NodeId"].ToString());
            form.NodeName = dr["NodeName"].ToString();
            form.NodeTypeId = int.Parse(dr["NodeTypeId"].ToString());
            form.NodeTypeName = dr["NodeTypeName"].ToString();
            form.EventDefId = int.Parse(dr["EventDefId"].ToString());
            form.EventDefName = dr["EventDefName"].ToString();
            form.StartTime = ((DateTime)dr["T3"]).ToString("yyyy-MM-dd HH:mm:ss");// (dr["T3"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T3"]); ;
            form.FinishTime = ((DateTime)dr["T1"]).ToString("yyyy-MM-dd HH:mm:ss"); //(dr["T1"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["T1"]);
            //form.PlanDuration = double.Parse(dr["PlanDuration"] == DBNull.Value ? "0" : dr["PlanDuration"].ToString());
            form.Duration = double.Parse(dr["ActualDuration"] == DBNull.Value ? "0" : dr["ActualDuration"].ToString());

            form.strDuration = ConvertMin2Time(form.Duration);

            return form;
        }
        /*public List<tblNodeType> GetNodeTypeLists(int iNodeType, out string NodeIds)
        {
            List<tblNodeType> lst = new NodeTypeDao().listAll(true);
            NodeIds = "";

            lst = lst.Where(x => x.Id == iNodeType).ToList();


            foreach (tblNodeType nodeType in lst)
            {
                NodeIds += nodeType.Id + ";";
            }

            return lst;
        }*/
        public IncForm GetEachInc(DataRow dr)
        {
            IncForm form = new IncForm();
            form.Year = int.Parse(dr["Year"].ToString());
            form.Month = int.Parse(dr["Month"].ToString());
            form.Day = int.Parse(dr["Day"].ToString());
            form.ActualDuration = Math.Round(double.Parse(dr["ActualDuration"] == DBNull.Value ? "0" : dr["ActualDuration"].ToString()));
            form.PlanDuration = double.Parse(dr["PlanDuration"] == DBNull.Value ? "0" : dr["PlanDuration"].ToString());


            return form;
        }
        public void GetLines(string LineId, out int iLineId)
        {
            iLineId = 0;
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
        }
        public void GetWorkShift(string WorkShift, out int iWorkShift)
        {
            int totalTimeWorkShift = 24 * 60;  // tong thoi gian cua ca lam viec 24h - ngay  hoac 8h - theo ca don vi phut
            iWorkShift = 0;
            if (!string.IsNullOrEmpty(WorkShift))
            {
                try
                {
                    iWorkShift = Convert.ToInt32(WorkShift);
                }
                catch { }
            }
            ViewBag.WorkShift = iWorkShift;

            List<tblShift> lstWorkShift = new WorkingShiftDao().listAll().Where(x => x.Id != 1).ToList(); // 1 ~ id Ca HC
            List<SelectListItem> WorkShifts = new List<SelectListItem>();
            foreach (var _workShift in lstWorkShift)
            {
                if (_workShift.Id == iWorkShift)
                {
                    totalTimeWorkShift = _workShift.TotalMinute;
                }
                WorkShifts.Add(new SelectListItem() { Value = _workShift.Id.ToString(), Text = _workShift.Name.ToString() });
            }
            WorkShifts.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "0" });
            ViewBag.WorkShifts = WorkShifts;
            ViewBag.totalTimeWorkShift = totalTimeWorkShift;
        }
        /*public void GetNodeType(string NodeType, out int iNodeType)
        {
            iNodeType = 0;
            if (!string.IsNullOrEmpty(NodeType))
            {
                iNodeType = Convert.ToInt32(NodeType);
            }
            ViewBag.NodeType = iNodeType;

            List<SelectListItem> NodeTypes = new List<SelectListItem>();
            List<tblNodeType> lst = new NodeTypeDao().listAll(true);


            foreach (var nt in lst)
            {
                NodeTypes.Add(new SelectListItem() { Value = nt.Id.ToString(), Text = nt.Name.ToString() });
            }

            NodeTypes.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "" });

            ViewBag.NodeTypes = NodeTypes;
        }*/
        public void GetNode(string Node, out int iNode)
        {
            iNode = 0;
            if (!string.IsNullOrEmpty(Node))
            {
                iNode = Convert.ToInt32(Node);
            }
            ViewBag.NodeId = iNode;

            List<SelectListItem> Nodes = new List<SelectListItem>();
            List<tblNode> lst = new NodeDao().listAll();

            foreach (var nt in lst)
            {
                Nodes.Add(new SelectListItem() { Value = nt.Id.ToString(), Text = nt.Name.ToString() });
            }
            Nodes.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "" });

            ViewBag.Nodes = Nodes;
        }
        public string ConvertMin2Time(double TotalMinute)
        {
            string ret = "";
            long _hour = (long)TotalMinute / 60;
            long _min = (long)TotalMinute % 60;

            ret += (_hour < 10 ? "0" + _hour.ToString() : _hour.ToString());
            ret += ":" + (_min < 10 ? "0" + _min.ToString() : _min.ToString());

            return ret;
        }
        public double ConvertMin2Hour(double TotalMinute)
        {
            //double ret = "";
            //long _hour = (long)TotalMinute / 60;
            //long _min = (long)TotalMinute % 60;

            //ret += (_hour < 10 ? "0" + _hour.ToString() : _hour.ToString());
            //ret += ":" + (_min < 10 ? "0" + _min.ToString() : _min.ToString());
            return Math.Round((double)TotalMinute / 60, 2);
        }
        #endregion





        public static DateTime GetFirstDayOfMonth(DateTime input)
        {
            DateTime result = input;
            result = result.AddDays((-result.Day) + 1);
            return result;
        }

        // BÁO CÁO KẾ HOẠCH SẢN XUẤT
        [HttpGet]
        public ActionResult ReportWorkPlan(string lineId, string startDate, string endDate, string productId)
        {
            int iLineId = 0;
            int iProductId = 0;
            GetNodeLine(lineId, out iLineId);
            GetProduct(productId, out iProductId);
            ViewBag.LineId = lineId;
            ViewBag.ProductId = productId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];

            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            var nowDate = DateTime.Now;
            var fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now.AddDays(-1);
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate.Trim());
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate.Trim());
            }
            
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");

            

            var result = new WorkOrderPlanDao().listAllByFilterReport(iLineId, iProductId, fromDate, toDate);


            return View("ReportWorkPlan",result);
        }
        [WebMethod]
        public JsonResult GetReportWorkPlan(string lineId, string startDate, string endDate)
        {

            int iLineId = 0;
            GetNodeLine(lineId, out iLineId);
            ViewBag.LineId = lineId;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate.Trim());
            }
            if (!string.IsNullOrEmpty(endDate))
            {

                toDate = Convert.ToDateTime(endDate.Trim());
            }
            List<tblWorkPlan> listWorkPlan = new WorkPlanDao().ListAllByFilterReport(iLineId, fromDate, toDate);
            foreach (tblWorkPlan item in listWorkPlan)
            {
                WorkPlanForm workPlanForm = new WorkPlanForm();
                workPlanForm.Cast(item);
            }
            List<WorkPlanForm> listWorkPlanForm = new List<WorkPlanForm>();
            using (SqlConnection conn = new SqlConnection(strConStr))
            {
                conn.Open();
                string query = "";
                DataTable data = new DataTable();
                SqlCommand sqlCommand = new SqlCommand(query, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(data);
                foreach (DataRow item in data.Rows)
                {
                    WorkPlanForm w = new WorkPlanForm();
                    w.LineCode = item["LineCode"] == null ? "" : item["LineCode"].ToString();
                    w.LineNames = item["LineName"] == null ? "" : item["LineName"].ToString();
                    w.PlanStartStr = item["PlanStart"] == null ? "" : Convert.ToDateTime(item["PlanStart"]).ToString("yyyy:MM:dd HH:mm:ss");
                    w.PlanFinishStr = item["PlanFinish"] == null ? "" : Convert.ToDateTime(item["PlanFinish"]).ToString("yyyy:MM:dd HH:mm:ss");
                    listWorkPlanForm.Add(w);
                }
                conn.Close();
            }

            return Json(listWorkPlanForm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult ExportReportWorkPlan(string startDate, string endDate, string lineId, string productId)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Today;
            DateTime toDate = DateTime.Today;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate.Trim());
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate.Trim());
            }
            int iLineId = 0;
            int iProductId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId.Trim());

            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId.Trim());
            }

            //string strFullPath = System.IO.Directory.GetCurrentDirectory();
            //string strFullPath= Environment.CurrentDirectory;

            string strFullPath= ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_kehoach_sanxuat.xlsx";
            string strFileName = strFullPath;
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            //Tạo list dữ liệu được filter theo điều kiện 
            //List<tblWorkPlan> listWp = new List<tblWorkPlan>();
            //listWp = new WorkPlanDao().ListAllByFilterReport(iLineId, fromDate, toDate);
            List<ReportWorkPlanModel> list = new List<ReportWorkPlanModel>();
            list = new WorkOrderPlanDao().listAllByFilterReport(iLineId, iProductId, fromDate, toDate);

            //Export ra excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                //WorkPlanForm dd = new WorkPlanForm();
                //dd.Cast(item);
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("F" + (startRow + iCount)).Value = item.ProductionName;
                ws.Cell("G" + (startRow + iCount)).Value = item.ProductCode;
                ws.Cell("H" + (startRow + iCount)).Value = item.Model;
                ws.Cell("I" + (startRow + iCount)).Value = item.ProductName;
                ws.Cell("J" + (startRow + iCount)).Value = item.PlanStart;
                ws.Cell("K" + (startRow + iCount)).Value = item.PlanQuantity;
                ws.Cell("L" + (startRow + iCount)).Value = item.Started;
                ws.Cell("M" + (startRow + iCount)).Value = item.ActualQuantity;
                ws.Cell("N" + (startRow + iCount)).Value = item.PerformancePercent;
                var strStatus = "";
                
                if (item.Status == 0)
                {
                    strStatus = "Kế hoạch";
                }
                if (item.Status == 2)
                {
                    strStatus = "Tạm dừng";
                }
                if (item.Status == 3)
                {
                    strStatus = "Hoàn thành";
                }
                if (item.Status == 1)
                {
                    strStatus = "Đang chạy";
                }
                ws.Cell("O" + (startRow + iCount)).Value = strStatus;

                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_KeHoach_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }

        //BÁO CÁO VẬN HÀNH
        [HttpGet]
        public ActionResult ReportOperational(string lineId, string startDate, string endDate, string nodeId)
        {
            int iLineId = 0;
            int iNodeId = 0;
            GetNodeLine(lineId, out iLineId);
            //GetNodeSecond(nodeId, out iNodeId);
            //GetNodeFromLineIdOne(lineId, out iLineId);
            ViewBag.LineId = lineId;
            ViewBag.NodeId = nodeId;

            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            List<SelectListItem> nodes = new List<SelectListItem>();
            List<tblNode> listn = new NodeDao().ListtblNodeByLineId(iLineId);
            foreach (var item in listn)
            {
                nodes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            nodes.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.Nodes = nodes;


            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];

            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(nodeId))
            {
                iNodeId = Convert.ToInt32(nodeId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now.AddDays(-1);
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");

            // Lọc dữ liệu cho báo cáo vận hành
            /*List<tblNodeEvent> listNodeEvent = new NodeEventDao().listAll();
            foreach (tblNodeEvent item in listNodeEvent)
            {
                NodeEventForm nodeForm = new NodeEventForm();
                nodeForm.Cast(item);
                listNodeEvent.Add(nodeForm);
            }
            ViewBag.NodeEvents = listNodeEvent;*/
            var result = new NodeEventDao().listByFilter(iLineId, iNodeId, fromDate, toDate);


            return View("ReportOperational", result);
        }
        [HttpGet]
        public FileContentResult ExportReportOperational(string lineId, string startDate, string endDate, string nodeId)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;
            int iNodeId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(nodeId))
            {
                iNodeId = Convert.ToInt32(nodeId);
            }
            //string strFullPath = ConfigPath;

            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_vanhanh_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            List<NodeOperationReportModel> list = new List<NodeOperationReportModel>();
            list = new NodeEventDao().listByFilter(iLineId, iNodeId, fromDate, toDate);

            //Export excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("F" + (startRow + iCount)).Value = item.NodeName;
                ws.Cell("G" + (startRow + iCount)).Value = item.WorkPlan;
                ws.Cell("H" + (startRow + iCount)).Value = item.RunningDuration;
                ws.Cell("I" + (startRow + iCount)).Value = item.StopDuration;
                ws.Cell("J" + (startRow + iCount)).Value = item.WaitDuration;
                ws.Cell("K" + (startRow + iCount)).Value = item.PerformancePercent;
                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_VanHanh_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }

        }

        //BÁO CÁO CHI TIẾT VẬN HÀNH
        [HttpGet]
        public ActionResult DetailedOperationReport(string lineId, string startDate, string endDate, string nodeId)
        {
            int iLineId = 0;
            int iNodeId = 0;
            GetNodeLine(lineId, out iLineId);
            /*GetNodeSecond(nodeId, out iNodeId);*/
            ViewBag.LineId = lineId;
            ViewBag.NodeId = nodeId;

            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            List<SelectListItem> nodes = new List<SelectListItem>();
            List<tblNode> listn = new NodeDao().ListtblNodeByLineId(iLineId);
            foreach (var item in listn)
            {
                nodes.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }
            nodes.Insert(0, new SelectListItem
            {
                Text = "--" + Resources.Language.All + "--",
                Value = ""
            });
            ViewBag.Nodes = nodes;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(nodeId))
            {
                iNodeId = Convert.ToInt32(nodeId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            // Lọc dữ liệu cho báo cáo chi tiết vận hành
            var result = new NodeEventDao().ListDetailByFiler(iLineId, iNodeId, fromDate, toDate);

            return View(result);
        }
        [HttpGet]
        public FileContentResult ExportDetailedOperationReport(string lineId, string startDate, string endDate, string nodeId)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;
            int iNodeId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(nodeId))
            {
                iNodeId = Convert.ToInt32(nodeId);
            }
            //string strFullPath = ConfigPath;

            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_vanhanhchitiet_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            List<DetailOperationReportModel> list = new NodeEventDao().ListDetailByFiler(iLineId, iNodeId, fromDate, toDate);

            //Export excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("F" + (startRow + iCount)).Value = item.NodeName;
                ws.Cell("G" + (startRow + iCount)).Value = item.EventDefName;
                ws.Cell("H" + (startRow + iCount)).Value = item.Started;
                ws.Cell("I" + (startRow + iCount)).Value = item.Ended;
                ws.Cell("J" + (startRow + iCount)).Value = item.ProcessDuration;
                
                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_VanHanhChiTiet_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }

        //BÁO CÁO SẢN LƯỢNG
        [HttpGet]
        public ActionResult QuantityReport(string lineId, string productId, string startDate, string endDate)
        {
            int iLineId = 0;
            int iProductId = 0;
            GetNodeLine(lineId, out iLineId);
            GetProduct(productId, out iProductId);
            ViewBag.LineId = lineId;
            ViewBag.NodeId = productId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            //Lọc dữ liệu cho báo cáo sản lượng
            var result = new ProductDao().ListbyFilterQuantityReport(iLineId, iProductId, fromDate, toDate);
            return View(result);
        }
        [HttpGet]
        public FileContentResult ExportQuantityReport(string lineId, string productId, string startDate, string endDate)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;
            int iProductId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            //string strFullPath = ConfigPath;

            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_sanluong_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            List<QuantityReportModel> list = new ProductDao().ListbyFilterQuantityReport(iLineId, iProductId, fromDate, toDate);

            //Export excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("F" + (startRow + iCount)).Value = item.ProductCode;
                ws.Cell("G" + (startRow + iCount)).Value = item.ProductName;
                ws.Cell("H" + (startRow + iCount)).Value = item.PlanDuration;
                ws.Cell("I" + (startRow + iCount)).Value = item.PlanQuantity;
                ws.Cell("J" + (startRow + iCount)).Value = item.ActualDuration;
                ws.Cell("K" + (startRow + iCount)).Value = item.ActualQuantity;
                ws.Cell("L" + (startRow + iCount)).Value = item.PerfromancePercent;
                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_SanLuong_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }

        //BÁO CÁO NĂNG SUẤT
        [HttpGet]
        public ActionResult ProductivityReport(string lineId, string productId, string startDate, string endDate)
        {
            int iLineId = 0;
            int iProductId = 0;
            GetNodeLine(lineId, out iLineId);
            GetProduct(productId, out iProductId);
            ViewBag.LineId = lineId;
            ViewBag.NodeId = productId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            //Lọc dữ liệu cho báo cáo năng suất
            var result = new LineProducttionDao().ListByFillter(iLineId, iProductId, fromDate, toDate);


            return View(result);
        }
        [HttpGet]
        public FileContentResult ExportProductivityReport(string lineId, string productId, string startDate, string endDate)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;
            int iProductId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            //string strFullPath = ConfigPath;

            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_nangsuat_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            List<ProductivityReportModel> list = new LineProducttionDao().ListByFillter(iLineId, iProductId, fromDate, toDate);

            //Export excel
            ws.Cell("E4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("D" + (startRow + iCount)).Value = item.ProductionName;
                ws.Cell("G" + (startRow + iCount)).Value = item.ProductCode;
                ws.Cell("H" + (startRow + iCount)).Value = item.Model;
                ws.Cell("I" + (startRow + iCount)).Value = item.ProductName;
                ws.Cell("J" + (startRow + iCount)).Value = item.PlanQuantity;
                ws.Cell("K" + (startRow + iCount)).Value = item.PlanDuration;
                ws.Cell("L" + (startRow + iCount)).Value = item.PlanUPH;
                ws.Cell("M" + (startRow + iCount)).Value = item.PlanHeadCount;
                ws.Cell("N" + (startRow + iCount)).Value = item.PlanUPPH;
                ws.Cell("O" + (startRow + iCount)).Value = item.ActualQuantity;
                ws.Cell("P" + (startRow + iCount)).Value = item.ActualDuration;
                ws.Cell("Q" + (startRow + iCount)).Value = item.ActualUPH;
                ws.Cell("R" + (startRow + iCount)).Value = item.ActualHeadCount;
                ws.Cell("S" + (startRow + iCount)).Value = item.ActualUPPH;
                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_NangSuat_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }

        //BÁO CÁO CHẤT LƯỢNG SẢN XUẤT
        [HttpGet]
        public ActionResult ProductionQualityReport(string lineId, string productId, string startDate, string endDate)
        {
            int iLineId = 0;
            int iProductId = 0;
            GetNodeLine(lineId, out iLineId);
            GetProduct(productId, out iProductId);
            ViewBag.LineId = lineId;
            ViewBag.NodeId = productId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            int iYear = reportDate.Year;
            int iMonth = reportDate.Month;
            int iDay = reportDate.Day;
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            DateTime today = DateTime.Now;
            DateTime fromDate = new DateTime(today.Year,today.Month,1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            var result = new ProductDao().ListbyFilterProductQuantityReport(iLineId, iProductId, fromDate, toDate);
            return View(result);
        }
        [HttpGet]
        public FileContentResult ExportProductionQualityReport(string lineId, string productId, string startDate, string endDate)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;
            int iProductId = 0;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            if (!string.IsNullOrEmpty(productId))
            {
                iProductId = Convert.ToInt32(productId);
            }
            //string strFullPath = ConfigPath;

            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_chatluong_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            //List<ProductionQualityReportModel> list = new List<ProductionQualityReportModel>();
            var list = new ProductDao().ListbyFilterProductQuantityReport(iLineId, iProductId, fromDate, toDate);
            //Export excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                //NodeForm node = new NodeForm();
               // node.Cast(item.);
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("F" + (startRow + iCount)).Value = item.ProductionName;
                ws.Cell("G" + (startRow + iCount)).Value = item.ProductCode;
                ws.Cell("H" + (startRow + iCount)).Value = item.Model;
                ws.Cell("I" + (startRow + iCount)).Value = item.ProductName;
                ws.Cell("J" + (startRow + iCount)).Value = item.ActualQuantiity;
                ws.Cell("K" + (startRow + iCount)).Value = item.FailQuantiity;
                ws.Cell("L" + (startRow + iCount)).Value = item.PassQuantiity;
                ws.Cell("M" + (startRow + iCount)).Value = item.Perfromance;

                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_ChatLuong_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }

        //BÁO CÁO OEE
        [HttpGet]
        public ActionResult OEEReport(string lineId, string startDate, string endDate)
        {
            int iLineId = 0;
            GetNodeLine(lineId, out iLineId);
            ViewBag.LineId = lineId;
            DateTime reportDate = DateTime.Now.AddDays(0 - Day2Close);
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            //Lọc dữ liệu cho báo cáo OEE
            var result = new OEEDao().ListOeeByFilterReport(iLineId, fromDate, toDate);

            return View(result);
        }
        [HttpGet]
        public FileContentResult ExportOEEReport(string lineId, string startDate, string endDate)
        {
            var session = (Model.DataModel.tblUser)Session[GlobalConstants.USER_SESSION];
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            int iLineId = 0;

            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }

            //string strFullPath = ConfigPath;


            string strFullPath = ControllerContext.HttpContext.Server.MapPath("~/");
            string strTemplateFile = strFullPath + @"\Reports\Templates\bc_OEE_sanxuat.xlsx";
            XLWorkbook wb = new XLWorkbook(strTemplateFile);
            IXLWorksheet ws = wb.Worksheet("Sheet1");
            int startRow = 7;
            int iCount = 1;

            // Tạo list dữ liệu filter
            List<OEEReportModel> list = new OEEDao().ListOeeByFilterReport(iLineId, fromDate, toDate);

            //Export excel
            ws.Cell("D4").Value = "Từ ngày " + fromDate.ToString("dd/MM/yyyy") + " đến ngày " + toDate.ToString("dd/MM/yyyy");

            foreach (var item in list)
            {
                
                ws.Cell("A" + (startRow + iCount)).Value = iCount;
                ws.Cell("B" + (startRow + iCount)).Value = item.Started;
                ws.Cell("C" + (startRow + iCount)).Value = item.LineCode;
                ws.Cell("D" + (startRow + iCount)).Value = item.LineName;
                ws.Cell("G" + (startRow + iCount)).Value = item.PlanDuration;
                ws.Cell("H" + (startRow + iCount)).Value = item.ActualDuration;
                ws.Cell("I" + (startRow + iCount)).Value = item.PlanPerformance;
                ws.Cell("J" + (startRow + iCount)).Value = item.PlanQuantity;
                ws.Cell("K" + (startRow + iCount)).Value = item.ActualQuantity;
                ws.Cell("L" + (startRow + iCount)).Value = item.ActualPerformance;
                ws.Cell("M" + (startRow + iCount)).Value = item.Pass;
                ws.Cell("N" + (startRow + iCount)).Value = item.NG;
                ws.Cell("O" + (startRow + iCount)).Value = item.Ratio;
                ws.Cell("P" + (startRow + iCount)).Value = item.OEE;
                
                if (iCount < list.Count)
                {
                    ws.Row(startRow + iCount).InsertRowsBelow(1);
                    iCount++;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao_OEE_SanXuat_" + DateTime.Now.ToString("yyyy/MM/dd") + ".xlsx");
            }
        }
    }
}