using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class OEEDao
    {
        AvaniDataContext db = null;
        public OEEDao()
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
        public List<OEEReportModel> ListOeeByFilterReport(int lineId, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join l in db.tblLines on wop.LineId equals l.Id
                        join wp in db.tblWorkPlans on wop.WorkPlanId equals wp.Id
                        where (wop.LineId == lineId || lineId == 0)
                        && Convert.ToDateTime(wop.PlanStart) >= startDate && Convert.ToDateTime(wop.PlanStart) <= endDate
                        && Convert.ToDateTime(wop.PlanFinish) >= startDate && Convert.ToDateTime(wop.PlanFinish) <= endDate
                        select new { wop, l, wp };
            var data = query.Select(x => new OEEReportModel()
            {
                LineCode = x.wop.LineCode,
                LineName = x.l.Name,
                Started = x.wop.Started,

                PlanDuration = x.wop.PlanDuration,
                ActualDuration = x.wop.ActualDuration == null || x.wop.ActualDuration == 0 ? 1 : x.wop.ActualDuration,
                PlanPerformance = Convert.ToDecimal((x.wop.ActualDuration == null ? 0 : x.wop.ActualDuration) / (x.wop.PlanDuration == 0 || x.wop.PlanDuration == null ? 1 : x.wop.PlanDuration)),

                PlanQuantity = x.wop.PlanQuantity,
                ActualQuantity = x.wop.ActualQuantity,
                ActualPerformance = Convert.ToDecimal((x.wop.ActualQuantity == null ? 0 : x.wop.ActualQuantity) / (x.wop.PlanQuantity == 0 || x.wop.PlanQuantity == null ? 1 : x.wop.PlanQuantity)),

                NG = x.wop.NG,
                Pass = (x.wop.ActualQuantity == null ? 0 : x.wop.ActualQuantity) - (x.wop.NG == null ? 0 : x.wop.NG),
                Ratio = Convert.ToDecimal(((x.wop.ActualQuantity == null ? 0 : x.wop.ActualQuantity) - (x.wop.NG == null ? 0 : x.wop.NG)) / ((x.wop.ActualQuantity == null || x.wop.ActualQuantity == 0 ? 3 : x.wop.ActualQuantity) - 2 * (x.wop.NG == null || x.wop.NG == 0 ? 1 : x.wop.NG))),

                OEE = (Convert.ToDecimal((x.wop.ActualDuration == null ? 0 : x.wop.ActualDuration) / (x.wop.PlanDuration == null ? 1 : x.wop.PlanDuration))) * (Convert.ToDecimal((x.wop.ActualQuantity == null ? 1 : x.wop.ActualQuantity) / (x.wop.PlanQuantity == 0 || x.wop.PlanQuantity == null ? 1 : x.wop.PlanQuantity))) * (Convert.ToDecimal(((x.wop.ActualQuantity == null ? 1 : x.wop.ActualQuantity) - (x.wop.NG == null ? 0 : x.wop.NG)) / (x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null ? 1 : x.wop.ActualQuantity)))
            }).ToList();
            return data;
        }
    }
}
