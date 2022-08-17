using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SummaryEventDao
    {
        string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        AvaniDataContext db = null;
        public SummaryEventDao()
        {
            if (!string.IsNullOrEmpty(Conn))
            {
                db = new AvaniDataContext(Conn);
            }else
            {
                db = new AvaniDataContext();
            }
        }

        public List<tblNodeSummaryEventReport> ListAll(int iYear, int iMonth, int iDay, int NumberOfDays, int WorkShift = 0, int Hour2UpdateReportDaily = 6)
        {
            List<tblNodeSummaryEventReport> lst = new List<tblNodeSummaryEventReport>();
            DateTime FinishDate = new DateTime(iYear, iMonth, iDay);
            //DateTime StartDate = FinishDate.AddDays(0 - NumberOfDays); //= 0 thì chỉ tính này hôm nay

            //string Conn = ConfigurationManager.ConnectionStrings["ConStr"].ToString(); ;
            //SqlConnection con = new SqlConnection(Conn);
            //con.Open();
            //string strSQL = "exec [CreateEventReport] @Year = '" + FinishDate.Year + "'";
            //SqlCommand cmd = new SqlCommand(strSQL, con);
            //cmd.ExecuteNonQuery();

            //for (int i = 0; i <= NumberOfDays; i++)
            //{
            //    DateTime d = FinishDate.AddDays(0 - i);

            //    //strSQL = " exec [CreateSummaryEventReport] @Year = " + d.Year + ", @Month = " + d.Year + ", @Day = " + d.Day;
            //    //cmd = new SqlCommand(strSQL, con);
            //    //cmd.ExecuteNonQuery();

            //    lst.AddRange(db.SummaryEventReports.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).ToList());
            //}
            using (SqlConnection con = new SqlConnection(Conn))
            {
                con.Open();

                for (int i = 0; i <= NumberOfDays; i++)
                {
                    DateTime d = FinishDate.AddDays(0 - i);

                    if (d.Date == DateTime.Now.Date)
                    {
                        //Nếu là ngày hôm nay thì cho nó cập nhật lại phần sự kiện trước khi lấy.
                        //exec [CreateEventReport] @Year = 2019, @Month = 7, @Day = 23, @Hour2Update = 6
                        string query = "exec [CreateEventReport] @Year = " + d.Year + ", @Month = " + d.Month + ", @Day = " + d.Day + ", @Hour2Update = " + Hour2UpdateReportDaily;

                        SqlCommand CreateCmd = new SqlCommand(query, con);
                        CreateCmd.ExecuteNonQuery();
                    }


                    string strSQL = " exec [CreateSummaryEventReport] @Year = " + d.Year + ", @Month = " + d.Month + ", @Day = " + d.Day;
                    strSQL += ",@WorkShift=" + WorkShift;
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtSource = new DataTable();

                    da.Fill(dtSource);

                    foreach (DataRow dr in dtSource.Rows)
                    {
                        tblNodeSummaryEventReport _form = new tblNodeSummaryEventReport();

                        _form = GetEachEvent(dr);
                        lst.Add(_form);
                    }
                    //lst.AddRange(db.SummaryEventReports.Where(x => x.Year == d.Year && x.Month == d.Month && x.Day == d.Day).ToList());
                }
            }

            return lst.OrderBy(x => x.NodeId).ToList();
        }

        private tblNodeSummaryEventReport GetEachEvent(DataRow dr)
        {
            tblNodeSummaryEventReport form = new tblNodeSummaryEventReport();

            form.Year = int.Parse(dr["Year"] == DBNull.Value ? "0" : dr["Year"].ToString());
            form.Month = int.Parse(dr["Month"] == DBNull.Value ? "0" : dr["Month"].ToString());
            form.Day = int.Parse(dr["Day"] == DBNull.Value ? "0" : dr["Day"].ToString());
            form.NodeId = int.Parse(dr["NodeId"].ToString());
            form.NodeName = dr["NodeName"].ToString();
            //form.NodeTypeId = int.Parse(dr["NodeTypeId"].ToString());
            //form.NodeTypeName = dr["NodeTypeName"].ToString();
            //form.NumberOfRunning = int.Parse(dr["NumberOfRunning"] == DBNull.Value ? "0" : dr["NumberOfRunning"].ToString());
            form.RunningDuration = double.Parse(dr["RunningDuration"] == DBNull.Value ? "0" : dr["RunningDuration"].ToString());
            //form.NumberOfStop = int.Parse(dr["NumberOfStop"] == DBNull.Value ? "0" : dr["NumberOfStop"].ToString());
            form.StopDuration = double.Parse(dr["StopDuration"] == DBNull.Value ? "0" : dr["StopDuration"].ToString());
            form.PlanDuration = long.Parse(dr["PlanDuration"] == DBNull.Value ? "0" : dr["PlanDuration"].ToString());

            return form;
        }
    }
}
