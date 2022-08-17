using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class LineProducttionDao
    {
        AvaniDataContext db = null;
        public LineProducttionDao()
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

        /*public List<tblLineProduction> listAll()
        {
            List<tblLineProduction> lst = new List<tblLineProduction>();
            lst = db.tblLineProductions.ToList();
            return lst.OrderBy(x=>x.LineId).ToList();
        }
        public tblLineProduction getOEEInProduction(int LineId,long WokingPlanId)
        {
            tblLineProduction lineProduction = db.tblLineProductions.Where(x=> x.WorkingPlanId!=null && x.WorkingPlanId == WokingPlanId && (x.LineId == LineId || LineId ==0)).OrderByDescending(x=>x.LogsTime).FirstOrDefault();
            return lineProduction;
        }

        public List<tblLineProduction> getByLineIdAndWorkingPlan(int LineId,long WokingPlanId)
        {
            List< tblLineProduction> lineProductions = db.tblLineProductions.Where(x => x.WorkingPlanId != null && x.WorkingPlanId == WokingPlanId && x.LineId == LineId).OrderByDescending(x => x.LogsTime).ToList();
            return lineProductions;
        }


        public tblLineProduction ViewDetail(int id)
        {
            try
            {
                return db.tblLineProductions.SingleOrDefault(x => x.LineId == id);
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
        }*/
        //public List<LineOnlineForm> LineJoinLineOnline()
        //{
        //   var result = (
        //                                   from l in db.tblLines
        //                                  join lo in db.tblLineOnlines on l.Id equals lo.LineId into yG
        //                                  from lL in yG.DefaultIfEmpty()
        //                                  select new LineOnlineForm()
        //                                  {
        //                                    LineId = l.Id,
        //                                    LineName = l.Name,
        //                                    nOrder = l.nOrder==null?0: Convert.ToInt32(l.nOrder),
        //                                    ShiftId = lL.ShiftId == null ? 0 : Convert.ToInt32(lL.ShiftId),
        //                                    StartTime = lL.StartTime == null ? DateTime.Now : Convert.ToDateTime(lL.StartTime),
        //                                    Status = lL.Status,
        //                                    HeadCount = lL.HeadCount,
        //                                    Plan = lL.Plan,
        //                                    Target = lL.Target,
        //                                    Actual = lL.Actual,
        //                                    NumberOfStop = lL.NumberOfStop,
        //                                    StopDuration = lL.StopDuration,
        //                                    Quality = lL.Quality
        //                                  }).ToList();
        //    return result;
        //}


        public List<ProductivityReportModel> ListByFillter(int lineId, int productId, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join l in db.tblLines on wop.LineId equals l.Id
                        join p in db.tblProducts on wop.ProductCode equals p.Code
                        join wp in db.tblWorkPlans on wop.WorkPlanId equals wp.Id
                        where (l.Id == lineId || lineId == 0) && (p.Id == productId || productId == 0)
                        && Convert.ToDateTime(wop.PlanStart) >= startDate && Convert.ToDateTime(wop.PlanStart) <= endDate
                        && Convert.ToDateTime(wop.PlanFinish) >= startDate && Convert.ToDateTime(wop.PlanFinish) <= endDate
                        select new { wop, l, p, wp };
            var data = query.Select(x => new ProductivityReportModel()
            {
                LineCode = x.wop.LineCode,
                LineName = x.l.Name,
                ProductionName = x.wop.ProductionName,
                ProductCode = x.wop.ProductCode,
                ProductName = x.wop.ProductName,
                Model = x.p.Model,

                //Plan
                PlanQuantity = x.wop.PlanQuantity,
                PlanDuration = x.wop.PlanDuration,
                PlanUPH = (x.wop.PlanQuantity) / (x.wop.PlanDuration == 0 ? 1 : x.wop.PlanDuration),
                PlanHeadCount = x.wp.PlanHeadCount,
                PlanUPPH = ((x.wop.PlanQuantity) / Convert.ToInt32((x.wop.PlanDuration == 0 ? 1 : x.wop.PlanDuration))) / (x.wp.PlanHeadCount == 0 ? 1 : x.wp.PlanHeadCount),

                //Actual
                ActualQuantity = x.wop.ActualQuantity,
                ActualDuration = x.wop.ActualDuration,
                ActualHeadCount = Convert.ToInt32(x.wp.ActualHeadCount == 0 ? 1 : x.wp.ActualHeadCount),
                ActualUPH = (x.wop.ActualQuantity) / Convert.ToInt32((x.wop.ActualDuration == 0 ? 1 : x.wop.ActualDuration)),
                ActualUPPH = ((x.wop.ActualQuantity) / Convert.ToInt32((x.wop.ActualDuration == 0 ? 1 : x.wop.ActualDuration))) / Convert.ToInt32(x.wp.ActualHeadCount == 0 ? 1 : x.wp.ActualHeadCount)
            }).ToList();
            return data;
        }
    }

}
