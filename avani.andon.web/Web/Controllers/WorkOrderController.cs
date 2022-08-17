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
using System.Data.OleDb;
using System.IO;

namespace avSVAW.Controllers
{
    public class WorkOrderController : BaseController
    {

        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index(string LineId, string Status, string startDate, string endDate)
        {
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
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            int iStatus = -1;
            if (!string.IsNullOrEmpty(Status))
            {
                iStatus = Convert.ToInt32(Status);
            }
            ViewBag.LineId = LineId;

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
            ViewBag.Statuss = iStatus;
            ViewBag.LineId = LineId;

            List<tblLine> lstLines = new LineDao().listAll().ToList(); // 1 ~ id Ca HC
            List<SelectListItem> Lines = new List<SelectListItem>();
            foreach (var _l in lstLines)
            {
                Lines.Add(new SelectListItem() { Value = _l.Id.ToString(), Text = _l.Name.ToString() });
            }
            Lines.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "0" });
            ViewBag.Lines = Lines;
            List<tblWorkOrder> lst = new WorkOrderDao().listAllByFilter(iLineId, iStatus, fromDate, toDate);
            List<WorkOrderForm> model = new List<WorkOrderForm>();
            foreach (tblWorkOrder l in lst)
            {
                WorkOrderForm lf = new WorkOrderForm();
                lf.Cast(l);
                model.Add(lf);
            }
            return View("Index", model);
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create()
        {
            WorkOrderForm model = new WorkOrderForm();
            model.Products = new ProductDao().ListAll().Select(n =>
                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(WorkOrderForm model)
        {
            if (ModelState.IsValid)
            {
                tblWorkOrder l = model.Cast();

                new WorkOrderDao().Insert(l);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long Id)
        {
            tblWorkOrder l = new WorkOrderDao().ViewDetail(Id);
            WorkOrderForm model = new WorkOrderForm();
            model.Cast(l);
            model.Products = new ProductDao().ListAll().Select(n =>
                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            WorkOrderDao d = new WorkOrderDao();
            tblWorkOrder model = d.ViewDetail(id);
            return View("Detail", model);
        }

        [HttpPost]
        public ActionResult Edit(WorkOrderForm model)
        {
            tblWorkOrder l = model.Cast();
            new WorkOrderDao().Update(l);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long Id)
        {
            new WorkOrderDao().Delete(Id);
            return RedirectToAction("Index");
        }

        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblWorkOrder> lst = new WorkOrderDao().listAll();
            return Json(lst);
        }
        /*[HttpPost]*/
        /*public ActionResult Importexcel1()
        {
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string connString = "";
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }
                    Request.Files["FileUpload1"].SaveAs(path1);
                    if (extension == ".csv")
                    {
                        ConvertCSVtoDataTable(path1);
                    }
                    //Connection String to Excel Workbook  
                    else if (extension.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        ConvertXSLXtoDataTable(path1, connString);

                    }
                    else if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        ConvertXSLXtoDataTable(path1, connString);
                    }

                }
                else
                {
                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
                }

            }
            return RedirectToAction("Index");
        }*/
        public void ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            tblProduct product = new tblProduct();
            ProductDao dao = new ProductDao();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                //foreach (string header in headers)
                //{
                //    dt.Columns.Add(header);
                //}

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        //castAndInsert(rows);
                    }
                }

            }
        }
        public void ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                object[] meta = new object[10];


                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                oledbConn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    bool read;
                    do
                    {
                        int NumberOfColums = reader.GetValues(meta);
                        string[] result = meta.Where(x => x != null)
                           .Select(x => x.ToString())
                           .ToArray();
                        //castAndInsert(result);
                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
            }
            catch
            {
            }
            finally
            {
                oledbConn.Close();
            }
        }
        /*public void castAndInsert(string[] arr)
        {
            tblWorkOrder workOrder = new tblWorkOrder();
            WorkOrderDao dao = new WorkOrderDao();
            if (arr.Length >= 10)
            {
                if (arr[0] != "")
                {
                    tblProduct product = new tblProduct();
                    ProductDao daoProduct = new ProductDao();
                    workOrder.ProductId = daoProduct.getIdByProductCode(arr[0]);
                    if (workOrder.ProductId == 0)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                if (arr[1] != "")
                {
                    try
                    {
                        workOrder.Quantity = Convert.ToInt32(arr[1]);
                    }
                    catch
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (arr[2] != "")
                {
                    try
                    {
                        workOrder.Deadline = Convert.ToDateTime(arr[2]);
                    }
                    catch
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (arr[3] != "")
                {
                    try
                    {
                        workOrder.Deadline = Convert.ToDateTime(arr[3]);
                    }
                    catch
                    {
                        
                    }
                }

                if (arr[4] != "")
                {
                    try
                    {
                        workOrder.PlanStart = Convert.ToDateTime(arr[4]);
                    }
                    catch
                    {

                    }
                }
                if (arr[5] != "")
                {
                    try
                    {
                        workOrder.PlanFinish = Convert.ToDateTime(arr[5]);
                    }
                    catch
                    {

                    }
                }
                if (arr[6] != "")
                {
                    try
                    {
                        workOrder.QuantityPlanned = Convert.ToInt32(arr[6]);
                    }
                    catch
                    {

                    }
                }if (arr[7] != "")
                {
                    try
                    {
                        workOrder.QuantityProduced = Convert.ToInt32(arr[7]);
                    }
                    catch
                    {

                    }
                }if (arr[8] != "")
                {
                    try
                    {
                        workOrder.PlanDuration = Convert.ToInt32(arr[8]);
                    }
                    catch
                    {

                    }
                }if (arr[9] != "")
                {
                    try
                    {
                        workOrder.ActualDuration = Convert.ToInt32(arr[9]);
                    }
                    catch
                    {

                    }
                }if (arr[10] != "")
                {
                    try
                    {
                        workOrder.Status = Convert.ToByte(arr[10]);
                    }
                    catch
                    {

                    }
                }

            }
            dao.Insert(workOrder);
        }*/

        [WebMethod]
        public JsonResult CheckExistCode(int Id, string Code)
        {
            int isExist = new WorkOrderDao().CheckExistCode(Id, Code);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }


    }
}