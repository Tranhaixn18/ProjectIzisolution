using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Model.Dao
{
    public class LineOnlineDao
    {
        AvaniDataContext db = null;
        public LineOnlineDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            }
            else
            {
                db = new AvaniDataContext();
            }
        }
        #region
        /*public List<tblLineOnline> listAll()
        {
            List<tblLineOnline> lst = new List<tblLineOnline>();
            lst = db.tblLineOnlines.ToList();
            return lst.OrderBy(x=>x.LineId).ToList();
        }
        
        public tblLineOnline ViewDetail(int id)
        {
            try
            {
                return db.tblLineOnlines.SingleOrDefault(x => x.LineId == id);
            }
            catch
            {
                return null;
            }
        }
        public string GetNameById(int id)
        {
            var obj = ViewDetail(id);
            return obj.LineName;
        }
        public bool Update(tblLineOnline entity)
        {
            try
            {

                string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

                var lineOnline = db.tblLineOnlines.SingleOrDefault(x => x.LineId == entity.LineId);

                int _lineId = lineOnline.LineId;
                long _workingPlanId = (int)lineOnline.WorkingPlanId;

                DateTime dt = DateTime.Now;
                if (dt.Hour < 6)
                {
                    dt = dt.AddDays(-1);
                }

                using (SqlConnection sqlConnection = new SqlConnection(con))
                {
                    sqlConnection.Open();
                    string strQuery = " EXEC UpdateLineOnlineValue ";
                    strQuery += "@LineId = " + _lineId + "";
                    strQuery += ", @WorkingPlanId = " + _workingPlanId + "";
                    strQuery += ", @PlanDate = '" + dt.ToString("yyyy-MM-dd") + "'";
                    strQuery += ", @Plan = " + entity.Plan + "";
                    strQuery += ", @Actual = " + entity.Actual + "";
                    strQuery += ", @StartTime = '" + ((DateTime)entity.StartTime).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    strQuery += ", @StopDuration = " + entity.StopDuration;
                    strQuery += ", @Takttime = " + entity.TakeTime;
                    strQuery += ", @Headcount = " + entity.HeadCount;
                    strQuery += ", @Quality = " + entity.Quality;

                    SqlCommand cmd = new SqlCommand(strQuery, sqlConnection);
                    cmd.ExecuteNonQuery();
                }


                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<LineOnlineForm> LineJoinLineOnline()
        {
           var result = (
                                           from l in db.tblLines
                                          join lo in db.tblLineOnlines on l.Id equals lo.LineId into yG
                                          from lL in yG.DefaultIfEmpty()
                                          select new LineOnlineForm()
                                          {
                                            LineId = l.Id,
                                            LineName = l.Name,
                                            nOrder = l.nOrder==null?0: Convert.ToInt32(l.nOrder),
                                            ShiftId = lL.ShiftId == null ? 0 : Convert.ToInt32(lL.ShiftId),
                                            StartTime = lL.StartTime == null ? DateTime.Now : Convert.ToDateTime(lL.StartTime),
                                            Status = lL.Status,
                                              RunningStatus = lL.RunningStatus,
                                            HeadCount = lL.HeadCount,
                                            Plan = lL.Plan,
                                            Target = lL.Target,
                                            Actual = lL.Actual,
                                            NumberOfStop = lL.NumberOfStop,
                                            StopDuration = lL.StopDuration,
                                            Quality = lL.Quality,
                                            OEE = lL.OEE,
                                            ProductModel = lL.ProductModel,
                                            WorkingPlanId = lL.WorkingPlanId
                                          }).ToList();
            return result;
        }*/
        #endregion

        public string getDate(string arr)
        {
            DateTime today = DateTime.Now;
            arr = String.Format("{0:dd/MM/yyyy}", today);
            return arr;
        }
        /*public static DateTime EndOfDay(this DateTime endDate)
        {
            return new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, 999);
        }*/
        public List<LineMornitorModel> GetLineMornitor(int lineId, int status, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join l in db.tblLines on wop.LineId equals l.Id
                        join p in db.tblProducts on wop.ProductCode equals p.Code
                        where (l.Id == lineId || lineId == 0)
                        && (Convert.ToDateTime(wop.PlanStart).Date >= startDate.Date) && (Convert.ToDateTime(wop.PlanStart).Date <= endDate.Date)
                        && (Convert.ToDateTime(wop.PlanFinish).Date >= startDate.Date) && (Convert.ToDateTime(wop.PlanFinish).Date <= endDate.Date)
                        && (wop.Status == status || status == -1)
                        select new { wop, l, p };
            var data = query.Select(x => new LineMornitorModel()
            {
                Id = x.wop.Id,
                LineCode = x.l.Code,
                LineName = x.l.Name,
                ProductCode = x.wop.ProductCode,
                ProductionName = x.wop.ProductionName,
                ProductName = x.wop.ProductName,
                ImageUrl = x.p.ImgUrl,
                PlanQuantity = x.wop.PlanQuantity,
                Started = x.wop.PlanStart,
                Ended = x.wop.PlanFinish,
                IsAuto = Convert.ToBoolean(x.l.IsAuto),
                Status = x.wop.Status
            }).OrderByDescending(x => x.Id).ToList();
            return data;
        }
        public int Update(tblWorkOrderPlan request)
        {
            try
            {
                var workPlan = db.tblWorkOrderPlans.SingleOrDefault(x => x.Id == request.Id);
                if (workPlan != null)
                {
                    workPlan.Status = request.Status;
                }
                db.SubmitChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int getLineId(string lineCode)
        {
            var line = db.tblLines.SingleOrDefault(x => x.Code == lineCode);
            if (line != null)
            {
                return line.Id;
            }
            else
            {
                return -1;
            }
        }

    }

}
