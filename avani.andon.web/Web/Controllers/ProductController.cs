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
using System.IO;
using System.Data.OleDb;
using PagedList;

namespace avSVAW.Controllers
{
    public class ProductController : BaseController
    {
        
        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index(int? page)
        {
            int currentPage = page ?? 1;
            int pageSize = 100;

            List<tblProduct> model = new ProductDao().ListAll();
            return View(model.ToPagedList(currentPage,pageSize));
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        public ActionResult Create()
        {
            ProductForm model = new ProductForm();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductForm model)
        {
            if (ModelState.IsValid)
            {
                tblProduct l = model.Cast();
                
                new ProductDao().Insert(l);
            }
            return RedirectToAction("Index");
        }

        //public ActionResult Edit(int Id)
        //{
        //    tblProduct l = new ProductDao().ViewDetail(Id);
        //    ProductForm model = new ProductForm();
        //    model.Cast(l);
        //    return View(model);
        //}


        [HttpPost]
        public ActionResult Edit(ProductForm model)
        {
            tblProduct l = model.Cast();
            new ProductDao().Update(l);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new ProductDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetEventDefJsonList()
        {
            List<tblProduct> lst = new ProductDao().ListAll();
            return Json(lst);
        }

        [HttpPost]
        public ActionResult Importexcel1()
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
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;\"";
                        ConvertXSLXtoDataTable(path1, connString);
                    }

                }
                else
                {
                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
                }

            }
            return RedirectToAction("Index");
        }
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
                        castAndInsert(rows);
                    }
                }

            }
        }
        public void  ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                object[] meta = new object[8];

              
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
                        castAndInsert(result);
                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                oledbConn.Close();
            }
        }
        public void castAndInsert(string[] arr) {
            tblProduct product = new tblProduct();
            ProductDao dao = new ProductDao();
            if (arr.Length >= 8)
            {
                if (arr[0] != "")
                {
                    product.Code = arr[0];
                }
                else
                {
                    return;
                }
                if (arr[1] != "")
                {
                    product.Name = arr[1];
                }
                else
                {
                    return;
                }
                if (arr[2] != "")
                {
                    product.Description = arr[2];
                } 
                if (arr[3] != "")
                {
                    product.Unit = arr[3];
                }
                if (arr[4] != "")
                {
                    product.Status =dao.getStatus(arr[4]);
                }
                if (arr[5] != "")
                {
                    try
                    {
                        product.TaktTime = Convert.ToInt32(arr[5]);
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
                if (arr[6] != "")
                {
                    try
                    {
                        product.CalculatedTaktTime = Convert.ToInt32(arr[6]);
                    }
                    catch
                    {
                        product.CalculatedTaktTime = 0;
                    }
                }if (arr[7] != "")
                {
                    try
                    {
                        product.Quantity = Convert.ToInt32(arr[7]);
                    }
                    catch
                    {
                        product.Quantity = 0;
                    }
                }
            }
            dao.Insert(product);
        }

        [WebMethod]
        public JsonResult CheckExistCode(int Id, string Code)
        {
            int isExist = new ProductDao().CheckExistCode(Id, Code);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}