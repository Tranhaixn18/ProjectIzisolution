using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class WorkingPlanDao
    {
        AvaniDataContext db = null;
        public WorkingPlanDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            }else
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
            catch (Exception e) { }
            return entity.Id;
        }

        public bool Update(tblWorkPlan entity)
        {
            try
            {
                var tblWorkPlan = db.tblWorkPlans.SingleOrDefault(x => x.Id == entity.Id);
                tblWorkPlan.ActualHeadCount = entity.ActualHeadCount;
                tblWorkPlan.ActualDuration = entity.ActualDuration;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }

        public bool UpdateOld(tblWorkPlan entity)
        {
            try
            {
                var tblWorkPlan = db.tblWorkPlans.SingleOrDefault(x => x.Id == entity.Id);
                //tblWorkPlan.WorkingId = entity.WorkingId;
                //tblWorkPlan.Year = entity.Year;
                //tblWorkPlan.Month = entity.Month;
                //tblWorkPlan.Day = entity.Day;
                //tblWorkPlan.ShiftId = entity.ShiftId;
                //tblWorkPlan.NodeId = entity.NodeId;

                tblWorkPlan.PlanStart = entity.PlanStart;

                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblWorkPlan> ListAll()
        {
            return db.tblWorkPlans.ToList();
        }
        /*public List<tblWorkPlan> ListAll(int iYear, int iMonth, int iDay, int iShift)
        {
            List<tblWorkPlan> lst = db.tblWorkPlans.Where(x => x.Year == iYear && x.Month == iMonth).ToList();
            if (iDay != 0)
            {
                lst = lst.Where(x => x.Day == iDay).ToList();
            }
            if (iShift != 0)
            {
                lst = lst.Where(x => x.ShiftId == iShift).ToList();
            }

            return lst;
        }*/
        /*public List<tblWorkPlan> ListAll(int iWorkingId)
        {
            List<tblWorkPlan> lst = db.tblWorkPlans.Where(x => x.WorkingId == iWorkingId).OrderBy(x => x.Day).ToList();
            return lst;
        }*/
        /*public List<tblWorkPlan> ListByFilter(int iYear,int iMonth,int iLineId,int iWorkingId)
        {
            List<tblWorkPlan> lst = db.tblWorkPlans.Where(x => x.Year == iYear 
            && x.Month==iMonth && (x.LineId == iLineId || iLineId == 0) &&(iWorkingId==0||x.WorkingId==iWorkingId) ).OrderBy(x => x.Day).ToList();
            return lst;
        }*/
        /*public List<tblWorkPlan> ListByKey(int iYear, int iMonth, int iDay, int iLineId)
        {
            List<tblWorkPlan> lst = db.tblWorkPlans.Where(x => x.Year == iYear && x.Month == iMonth && x.Day==iDay &&(iLineId==0|| x.LineId==iLineId)).ToList();

            return lst;
        }*/
        /*public List<tblWorkPlan> ListByDate(DateTime DDate, int iLineId)
        {
            List<tblWorkPlan> lst = db.tblWorkPlans.Where(x => x.Year == DDate.Year && x.Month == DDate.Month && x.Day == DDate.Day && (iLineId==0 || iLineId==x.LineId)).ToList();
            return lst;
        }*/
        /*public int MaxWorkingId()
        {
            int result = 0;
            try
            {
                //result = db.Receipts.Count(x => x.ReceiptDate.Year.Equals(DateTime.Today.Year) &&
                //                                x.ReceiptDate.Month.Equals(DateTime.Today.Month));

                result = (int)db.tblWorkPlans.Max(x => x.WorkingId);

            }
            catch
            {

            }
            return result;
        }*/

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
        public tblWorkOrder ViewDetailSecond(int id)
        {
            try
            {
                return db.tblWorkOrders.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        /*public bool DeleteWorking(int id)
        {
            try
            {
                var line = db.tblWorkPlans.Where(x => x.WorkingId == id);
                db.tblWorkPlans.DeleteAllOnSubmit(line);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }*/

        /*public bool CheckDuplicate(tblWorkPlan entity)
        {
            try
            {
                var tblWorkPlan = db.tblWorkPlans.SingleOrDefault(x => x.Year == entity.Year && (x.Month == entity.Month) && (x.Day == entity.Day) && (x.ShiftId == entity.ShiftId) && (x.NodeId == entity.NodeId));
                return (tblWorkPlan != null);
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }*/

        public bool Delete(int id)
        {
            try
            {
                var line = db.tblWorkPlans.SingleOrDefault(x => x.Id == id);
                db.tblWorkPlans.DeleteOnSubmit(line);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
