using Model.DataModel;
using Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using avSVAW.Models;

namespace Model.Dao
{
    public class WorkPlanDao
    {
        AvaniDataContext db = null;
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public WorkPlanDao()
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

        public long Insert(tblWorkPlan entity)
        {
            try
            {
                db.tblWorkPlans.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch { }
            return entity.Id;
        }

        public bool Update(tblWorkPlan entity)
        {
            try
            {
                var tblWorkPlan = db.tblWorkPlans.SingleOrDefault(x => x.Id == entity.Id);
                tblWorkPlan.PlanHeadCount = entity.PlanHeadCount;
                tblWorkPlan.LineId = entity.LineId;
                tblWorkPlan.PlanBreakDuration = entity.PlanBreakDuration;
                //tblWorkPlan.PlanBreakFinish1 = entity.PlanBreakFinish1;
                //tblWorkPlan.PlanBreakFinish2 = entity.PlanBreakFinish2;
                //tblWorkPlan.PlanBreakFinish3 = entity.PlanBreakFinish3;
                //tblWorkPlan.PlanBreakStart1 = entity.PlanBreakStart1;
                ////tblWorkPlan.PlanBreakStart2 = entity.PlanBreakStart2;
                //tblWorkPlan.PlanBreakStart3 = entity.PlanBreakStart3;
                tblWorkPlan.PlanFinish = entity.PlanFinish;
                tblWorkPlan.PlanStart = entity.PlanStart;
                tblWorkPlan.PlanTotalDuration = entity.PlanTotalDuration;
                tblWorkPlan.PlanWorkingDuration = entity.PlanWorkingDuration;
                tblWorkPlan.Priority = entity.Priority;
                tblWorkPlan.Status = entity.Status;
                //tblWorkPlan.EmployeeId = entity.EmployeeId;
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblWorkPlan> listAll()
        {
            return db.tblWorkPlans.ToList();
        }
        public List<tblWorkPlan> ListAllByFilter(int LineId, int Year, int Month, int Status)
        {
            
            return db.tblWorkPlans.Where(p =>
            (p.LineId == LineId || LineId == 0)
            && (Convert.ToDateTime(p.PlanStart).Year == Year || Year == 0)
            && (Convert.ToDateTime(p.PlanStart).Month == Month || Month == 0)
            && (p.Status == Status || Status == -1)).OrderByDescending(p => p.Id).ToList();
        }

        public List<WorkPlanIndex> ListAllByFilter2(int LineId, int Year, int Month)
        {

            var query = from wp in db.tblWorkPlans
                        join wop in db.tblWorkOrderPlans on wp.Id equals wop.WorkPlanId into wp2
                        from tblWorkOrderPlan in wp2.DefaultIfEmpty()
                        join l in db.tblLines on wp.LineCode equals l.Code
                        where wp.LineId == LineId || LineId == 0 && (Convert.ToDateTime(wp.PlanStart).Year == Year || Year == 0) && (Convert.ToDateTime(wp.PlanStart).Month == Month || Month == 0)
                        select new { wp, tblWorkOrderPlan, l };
            var data = query.Select(x => new WorkPlanIndex()
            {
                LineName = x.l.Name,
                ProductionName = x.tblWorkOrderPlan.ProductionName,
                PlanStart = x.wp.PlanStart,
                PlanFinish = x.wp.PlanFinish,
                Id = x.wp.Id,
            }).ToList();
            return data;
        }
        public List<tblWorkPlan> ListAllByFilterReport(int LineId, DateTime start, DateTime end)
        {
            return db.tblWorkPlans.Where(p =>
            (p.LineId == LineId || LineId == 0)
            && Convert.ToDateTime(p.PlanStart) >= start && Convert.ToDateTime(p.PlanStart) <= end
            && Convert.ToDateTime(p.PlanFinish) >= start && Convert.ToDateTime(p.PlanFinish) <= end

            && p.Status != 0

            ).ToList();
        }
        public tblWorkPlan ViewDetail(int id)
        {
            try
            {
                return db.tblWorkPlans.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var WorkPlan = db.tblWorkPlans.SingleOrDefault(x => x.Id == id);
                db.tblWorkPlans.DeleteOnSubmit(WorkPlan);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public int MaxWorkPlanId()
        {
            int result = 0;
            try
            {
                //result = db.Receipts.Count(x => x.ReceiptDate.Year.Equals(DateTime.Today.Year) &&
                //                                x.ReceiptDate.Month.Equals(DateTime.Today.Month));

                result = (int)db.tblWorkPlans.Max(x => x.Id);

            }
            catch
            {

            }
            return result;
        }
    }


}
