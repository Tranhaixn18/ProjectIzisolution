using avSVAW.Models;
using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Model.Dao
{
    public class WorkOrderPlanDao
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        AvaniDataContext db = null;
        public const int STATUS0 = 0;
        public const int STATUS1 = 1;
        public const int STATUS2 = 2;
        public const int STATUS3 = 3;
        public string getStatusName(byte Status)
        {
            string status = "";
            if (Status == STATUS0)
            {
                status = "Kế hoạch";
            }
            else if (Status == STATUS1)
            {
                status = "Đang chạy";
            }
            else if (Status == STATUS2)
            {
                status = "Tạm dừng";
            }
            else if (Status == STATUS3)
            {
                status = "Hoàn thành";
            }
            return status;
        }
        public WorkOrderPlanDao()
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

        /*public void BraodcastSocketIO()
        {
            var dataList = "request";
            WorkOrderPlanHub.WorkOrderPlanBoardcast(dataList);
        }*/

        public void Insert(tblWorkOrderPlan entity)
        {
            List<tblWorkOrderPlan> UNexIDs = new List<tblWorkOrderPlan>();
            if (entity.WorkPlanId != null)
            {
                UNexIDs = getMaxIdByWorkPlanId(Convert.ToInt32(entity.WorkPlanId));
            }
            int LineId = 0;
            tblWorkPlan wp = new WorkPlanDao().ViewDetail(Convert.ToInt32(entity.WorkPlanId));
            if (wp != null)
            {
                if (wp.LineId != null)
                {
                    LineId = Convert.ToInt32(wp.LineId);
                }
            }

            try
            {
                List<tblNode> lstNodes = new NodeDao().ListtblNodeByLineId(LineId).ToList();
                List<tblWorkOrderPlan> entities = new List<tblWorkOrderPlan>();
                foreach (tblNode n in lstNodes)
                {
                    tblWorkOrderPlan t = new tblWorkOrderPlan();
                    t = castInsert(entity);
                    //t.NodeId = n.Id;
                    entities.Add(t);
                }

                db.tblWorkOrderPlans.InsertAllOnSubmit(entities);
                db.SubmitChanges();

                List<tblWorkOrderPlan> updateNexIds = new List<tblWorkOrderPlan>();
                foreach (tblWorkOrderPlan unext in UNexIDs)
                {
                    tblWorkOrderPlan wop = entities.Find(p => p.WorkPlanId == unext.WorkPlanId /*&& p.NodeId == unext.NodeId*/);
                    if (wop != null)
                    {
                        unext.NextId = (int)entities.Find(p => p.WorkPlanId == unext.WorkPlanId /*&& p.NodeId == unext.NodeId*/).Id;
                    }
                    db.SubmitChanges();
                }
            }
            catch { }
        }

        public long InsertForSocket(tblWorkOrderPlan entity)
        {
            db.tblWorkOrderPlans.InsertOnSubmit(entity);
            db.SubmitChanges();
            return entity.Id;

        }
        public void UpdateEntity(tblWorkOrderPlan node)
        {
            try
            {
                tblWorkOrderPlan entity = ViewDetail(node.Id);
                entity.ActualTaktTime = node.ActualTaktTime;
                //entity.Duration = node.Duration;
                entity.Finished = node.Finished;
                entity.HeadCount = node.HeadCount;
                entity.NextId = node.NextId;
                entity.nOrder = node.nOrder;
                entity.PlanFinish = node.PlanFinish;
                entity.PlanStart = node.PlanStart;
                //entity.Quantity = node.Quantity;
                //entity.QuantityProduced = node.QuantityProduced;
                entity.Started = node.Started;
                entity.Status = node.Status;
                entity.TaktTime = node.TaktTime;
                entity.UPH = node.UPH;
                entity.UPPH = node.UPPH;
                //entity.WorkOrderId = node.WorkOrderId;
                entity.WorkPlanId = node.WorkPlanId;
                //entity.Speed = node.Speed;
                db.SubmitChanges();
            }
            catch { }
        }
        public void UpdateStatusToStop(tblWorkOrderPlan wop)
        {
            var objectWop = db.tblWorkOrderPlans.FirstOrDefault(x => x.Id == wop.Id);
            objectWop.Status = 1;
            db.SubmitChanges();
        }
        public void UpdateStatusToRun(tblWorkOrderPlan wop)
        {
            var objectWop = db.tblWorkOrderPlans.FirstOrDefault(x => x.Id == wop.Id);
            objectWop.Status = 2;
            db.SubmitChanges();
        }
        public bool Update(tblWorkOrderPlan node)
        {
            try
            {
                int LineId = 0;
                tblWorkPlan wp = new WorkPlanDao().ViewDetail(Convert.ToInt32(node.WorkPlanId));
                if (wp != null)
                {
                    if (wp.LineId != null)
                    {
                        LineId = Convert.ToInt32(wp.LineId);
                    }
                }
                var lst = db.tblWorkOrderPlans.Where(x => x.WorkPlanId == node.WorkPlanId /*&& x.WorkOrderId == node.WorkOrderId*/).ToList();
                foreach (tblWorkOrderPlan entity in lst)
                {
                    entity.ActualTaktTime = node.ActualTaktTime;
                    //entity.Duration = node.Duration;
                    entity.Finished = node.Finished;
                    entity.HeadCount = node.HeadCount;
                    entity.NextId = node.NextId;
                    entity.nOrder = node.nOrder;
                    entity.PlanFinish = node.PlanFinish;
                    entity.PlanStart = node.PlanStart;
                    //entity.Quantity = node.Quantity;
                    //entity.QuantityProduced = node.QuantityProduced;
                    entity.Started = node.Started;
                    entity.Status = node.Status;
                    entity.TaktTime = node.TaktTime;
                    entity.UPH = node.UPH;
                    entity.UPPH = node.UPPH;
                    //entity.WorkOrderId = node.WorkOrderId;
                    entity.WorkPlanId = node.WorkPlanId;
                    //entity.Speed = node.Speed;
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }

        public List<tblWorkOrderPlan> listAll()
        {
            return db.tblWorkOrderPlans.OrderBy(x => x.nOrder).ToList();
        }
        public List<tblWorkOrderPlan> listAllByFilter(long workPlanId)
        {
            return db.tblWorkOrderPlans.Where(x => x.WorkPlanId == workPlanId).GroupBy(c => new
            {
                //c.WorkOrderId,
                c.WorkPlanId
            }).Select(x => x.First()).OrderBy(p => p.nOrder).ToList();

        }
        public List<tblWorkOrderPlan> listAllByFilter2(long workPlanId)
        {
            return db.tblWorkOrderPlans.Where(x => x.WorkPlanId == workPlanId).OrderByDescending(p => p.Id).ToList();
        }

        public tblWorkOrderPlan ViewDetail(long id)
        {
            try
            {
                return db.tblWorkOrderPlans.SingleOrDefault(x => x.WorkPlanId == id);
            }
            catch
            {
                return null;
            }
        }
        public tblWorkOrderPlan ViewDetail2(long id)
        {
            try
            {
                return db.tblWorkOrderPlans.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public tblWorkOrder ViewDetailForLineCode(string lineCode)
        {
            try
            {
                return db.tblWorkOrders.SingleOrDefault(x => x.LineCode == lineCode);
            }
            catch
            {
                return null;
            }
        }
        public List<tblWorkOrderPlan> ViewListDetail(long id)
        {
            try
            {
                return db.tblWorkOrderPlans.Where(x => x.WorkPlanId == id).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<tblWorkOrderPlan> getByWorkPlanId(long WorkPlanId)
        {
            return db.tblWorkOrderPlans.Where(x => x.WorkPlanId == WorkPlanId).OrderBy(x => x.nOrder).ToList();
        }
        public int getByWorkPlanIdSQL(long WorkPlanId)
        {
            using (SqlConnection con = new SqlConnection(strConStr))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM tblWorkOrderPlan WHERE WorkPlanId = " + WorkPlanId;
                SqlCommand command = new SqlCommand(query, con);
                var rowCount = Convert.ToInt32(command.ExecuteScalar());
                return rowCount;
            }
        }
        public List<tblWorkOrderPlan> getByFilter(int[] WorkPlanId, int NodeId, string Product)
        {
            return db.tblWorkOrderPlans.Where(x => WorkPlanId.Contains(Convert.ToInt32(x.WorkPlanId)) /*&& (NodeId==0|| NodeId==x.NodeId)*/
            && (Product == "" || Product.Contains(x.ProductCode))
            ).OrderBy(x => x.nOrder).ToList();
        }

        public List<tblWorkOrderPlan> getMaxIdByWorkPlanId(int WorkPlanId)
        {
            tblWorkOrderPlan wop = db.tblWorkOrderPlans.Where(x => x.WorkPlanId == WorkPlanId).OrderByDescending(x => x.nOrder).FirstOrDefault();
            if (wop != null)
            {
                int nOrderMax = Convert.ToInt32(wop.nOrder);
                return db.tblWorkOrderPlans.Where(x => x.WorkPlanId == WorkPlanId && x.nOrder == nOrderMax).ToList();
            }
            else
            {
                return new List<tblWorkOrderPlan>();
            }


        }
        public bool Delete(int WorkOrderId, int WorkPlanId)
        {
            try
            {
                var WorkOrderPlan = db.tblWorkOrderPlans.Where(/*x => x.WorkOrderId == WorkOrderId &&*/ x => x.WorkPlanId == WorkPlanId).ToList();
                db.tblWorkOrderPlans.DeleteAllOnSubmit(WorkOrderPlan);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public tblWorkOrderPlan castInsert(tblWorkOrderPlan node)
        {
            int QuantityProcedure = 0;
            if (ConfigurationManager.AppSettings["QUANTITY_SUB"] != null)
            {
                try
                {
                    QuantityProcedure = Convert.ToInt32(ConfigurationManager.AppSettings["QUANTITY_SUB"].ToString());
                }
                catch
                {

                }
            }

            return new tblWorkOrderPlan()
            {
                nOrder = node.nOrder,
                ActualTaktTime = node.ActualTaktTime,
                //Duration = node.Duration,
                Finished = node.Finished,
                HeadCount = node.HeadCount,
                NextId = node.NextId,
                PlanFinish = node.PlanFinish,
                PlanStart = node.PlanStart,
                //Quantity = node.Quantity,
                //QuantityProduced = QuantityProcedure,//default -27
                Started = node.Started,
                Status = STATUS0, //fix 
                TaktTime = node.TaktTime,
                UPH = node.UPH,
                UPPH = node.UPPH,
                //WorkOrderId = node.WorkOrderId,
                ProductCode = node.ProductCode,
                ProductName = node.ProductName,
                WorkOrderCode = node.WorkOrderCode,
                //Speed = node.Speed,
                WorkPlanId = node.WorkPlanId
            };
        }


        public List<ReportWorkPlanModel> listAllByFilterReport(int lineId, int productId, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join wo in db.tblWorkOrders on wop.WorkOrderCode equals wo.Code
                        join l in db.tblLines on wop.LineId equals l.Id
                        join p in db.tblProducts on wo.ProductCode equals p.Code
                        where (wop.LineId == lineId || lineId == 0) && (p.Id == productId || productId == 0)
                        && Convert.ToDateTime(wop.PlanStart) >= startDate && Convert.ToDateTime(wop.PlanStart) <= endDate
                        select new { wop, wo, l, p };
            var data = query.Select(x => new ReportWorkPlanModel()
            {
                LineCode = x.l.Code,
                LineName = x.l.Name,
                ProductionName = x.wo.ProductionName,
                ProductCode = x.p.Code,
                Model = x.p.Model,
                ProductName = x.p.Name,
                PlanStart = x.wop.PlanStart,
                PlanQuantity = x.wop.PlanQuantity,
                Started = x.wop.Started,
                ActualQuantity = x.wop.ActualQuantity,
                Status = x.wo.Status,
                Performance = (((Convert.ToDecimal(x.wop.ActualQuantity)) / (Convert.ToDecimal(x.wop.PlanQuantity)))) * 100,
                PerformancePercent = (Convert.ToDecimal(x.wop.ActualQuantity)) / (Convert.ToDecimal(x.wop.PlanQuantity))
            }).ToList();
            return data;
        }


    }
}
