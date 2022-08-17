using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Dao
{
    public class NodeEventDao
    {
        AvaniDataContext db = null;
        public NodeEventDao()
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

        /*public long Insert(tblNodeEvent entity)
        {
            try
            {
                db.tblNodeEvents.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception)
            {
            }
            return entity.Id;
        }*/

        /*public bool Update(tblNodeEvent entity)
        {
            try
            {
                var tblNodeEvent = db.tblNodeEvents.SingleOrDefault(x => x.Id == entity.Id);

                tblNodeEvent.NodeId = entity.NodeId;
                tblNodeEvent.Reason = entity.Reason;
                tblNodeEvent.T1 = entity.T1;
                tblNodeEvent.T2 = entity.T2;
                tblNodeEvent.T3 = entity.T3;
                //tblNodeEvent.T4 = entity.T4;
                tblNodeEvent.EventDefId = entity.EventDefId;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }*/

        //public List<tblNode> listAll()
        //{
        //    return db.tblNodes.ToList();
        //}
        /*public List<tblNodeEvent> listAll()
        {
            return db.tblNodeEvents.ToList();
        }*/
        /*public List<tblNodeEvent> listByDate(
            int startYear, int startMonth, int startDay, int startHours, int startMinute,
            int endYear, int endMonth, int endDay, int endHours, int endMinute
            )
        {
            DateTime startDate = new DateTime(startYear, startMonth, startDay, startHours, startMinute, 0);
            DateTime endDate = new DateTime(endYear, endMonth, endDay, endHours, endMinute, 0);
            var NodeEvents = db.tblNodeEvents.Where(x => x.T3 != null &&
            Convert.ToDateTime(x.T3) >= startDate && Convert.ToDateTime(x.T3) <= endDate).OrderByDescending(x => x.T3);
            return NodeEvents.ToList();
        }*/
        public List<NodeOperationReportModel> listByFilter(int LineId, int NodeId, DateTime startDate, DateTime endDate)
        {

            var query = from nor in db.tblNodeOperationReports
                        join l in db.tblLines on nor.LineId equals l.Id
                        join n in db.tblNodes on nor.NodeId equals n.Id
                        join wp in db.tblWorkPlans on nor.WorkingPlanId equals wp.Id
                        join nser in db.tblNodeSummaryEventReports on nor.NodeId equals nser.NodeId
                        where (l.Id == LineId || LineId == 0) && (n.Id == NodeId || NodeId == 0)
                        && Convert.ToDateTime(nor.T3) >= startDate && Convert.ToDateTime(nor.T3) <= endDate
                        && Convert.ToDateTime(nor.T1) >= startDate && Convert.ToDateTime(nor.T1) <= endDate
                        select new { nor, l, n , wp, nser};
            var data = query.Select(x => new NodeOperationReportModel()
            {
                NodeId=x.n.Id,
                LineCode = x.l.Code,
                LineName = x.l.Name,
                NodeName = x.n.Name,
                WorkPlan=(x.wp.PlanTotalDuration),
                RunningDuration=x.nser.RunningDuration,
                StopDuration=x.nser.StopDuration,
                WaitDuration=x.nor.WaitDuration,
                Performance= (Convert.ToDecimal(x.nser.RunningDuration)/Convert.ToDecimal(x.wp.PlanTotalDuration))*100,
                PerformancePercent =Convert.ToDecimal(x.nser.RunningDuration)/Convert.ToDecimal(x.wp.PlanTotalDuration)

            }).ToList();
            return data;
        }

        public List<DetailOperationReportModel> ListDetailByFiler(int lineId, int nodeId, DateTime startDate, DateTime endDate)
        {
            var query = from nor in db.tblNodeOperationReports
                        join l in db.tblLines on nor.LineId equals l.Id
                        join n in db.tblNodes on nor.NodeId equals n.Id
                        join ev in db.tblEventDefs on nor.EventDefId equals ev.Id
                        where (l.Id == lineId || lineId == 0) && (n.Id == nodeId || nodeId == 0)
                        && Convert.ToDateTime(nor.T3) >= startDate && Convert.ToDateTime(nor.T3) <= endDate
                        && Convert.ToDateTime(nor.T1) >= startDate && Convert.ToDateTime(nor.T1) <= endDate
                        select new { nor, l, n , ev};
            var data = query.Select(x => new DetailOperationReportModel()
            {
                LineCode = x.l.Code,
                LineName = x.l.Name,
                NodeName = x.nor.NodeName,
                EventDefName = x.ev.Name_VN,
                Started = x.nor.T3,
                Ended = x.nor.T1,
                ProcessDuration = (Convert.ToDateTime(x.nor.T1) - Convert.ToDateTime(x.nor.T3))
            }).ToList();
            return data;
        }

        /*public tblNodeEvent ViewDetail(int id)
        {
            try
            {
                return db.tblNodeEvents.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }*/


        /*public bool Delete(int id)
        {
            try
            {
                var node = db.tblNodeEvents.SingleOrDefault(x => x.Id == id);
                db.tblNodeEvents.DeleteOnSubmit(node);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }*/


        /*public List<tblNodeEvent> listByFilterForTimeLine(int LineId, int NodeId,
           int startYear, int startMonth, int startDay, int startHours, int startMinute,
           int endYear, int endMonth, int endDay, int endHours, int endMinute
           )
        {
            DateTime startDate = new DateTime(startYear, startMonth, startDay, startHours, startMinute, 0);
            DateTime endDate = new DateTime(endYear, endMonth, endDay + 1, endHours, endMinute, 0);
            List<tblNode> lstNode = new NodeDao().ListtblNodeByLineId(LineId);
            int[] nodein = new int[lstNode.Count];
            int i = 0;
            foreach (tblNode n in lstNode)
            {
                nodein[i] = n.Id;
                i++;
            }

            var NodeEvents = db.tblNodeEvents.Where(
                x => (LineId == 0 || nodein.Contains(Convert.ToInt32(x.NodeId)))
            && (x.NodeId == NodeId || NodeId == 0) && x.T3 != null &&
            Convert.ToDateTime(x.T3) >= startDate && Convert.ToDateTime(x.T3) <= endDate &&
           (x.T1 == null || (x.T1 != null && Convert.ToDateTime(x.T1) >= startDate && Convert.ToDateTime(x.T1) <= endDate)
            ))
                .OrderBy(x => x.T3).ThenBy(x => x.T1);
            return NodeEvents.ToList();
        }*/

    }
}
