using Common;
using Model.Dao;
using Model.DataModel;
using avSVAW.App_Start;
using avSVAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Model.Models;

namespace avSVAW.Controllers
{
    [SessionTimeout]
    public class HomeController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        public ActionResult Index()
        {
           


            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }
        public ActionResult Summary()
        {
           
            return PartialView("_Summary");
        }

        private DateTime GetLastestWorkingDay()
        {
            DateTime Yesterday = DateTime.Now;
            int iCount = 0;
            //using (SqlConnection con = new SqlConnection(strConStr))
            //{
            //    con.Open();
            //    do
            //    {
            //        Yesterday = Yesterday.AddDays(-1);

            //        string strQuery = "SELECT * FROM tblWorkingPlan WHERE [Year] = " + Yesterday.Year + " AND [Month] = " + Yesterday.Month + " AND [Day] = " + Yesterday.Day;

            //        DataTable dt = new DataTable();
            //        SqlCommand cmd = new SqlCommand(strQuery, con);
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        DataSet ds = new DataSet();
            //        da.Fill(ds);
            //        dt = ds.Tables[ds.Tables.Count - 1];
            //        iCount = dt.Rows.Count;
            //    }
            //    while (iCount <= 0);

            //}

            return Yesterday;
        }

    }
}
