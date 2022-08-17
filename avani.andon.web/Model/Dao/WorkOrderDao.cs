using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class WorkOrderDao
    {
        AvaniDataContext db = null;

        public const int STATUS0 = 0;
        public const int STATUS1 = 1;
        public const int STATUS2 = 2;
        public const int STATUS3 = 3;
        public const int STATUS4 = 4;
        public WorkOrderDao()
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

        public long Insert(tblWorkOrder entity)
        {
            try
            {
                db.tblWorkOrders.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch { }
            return entity.Id;
        }

        public bool Update(tblWorkOrder entity)
        {
            try
            {
                var tblWorkOrder = db.tblWorkOrders.SingleOrDefault(x => x.Id == entity.Id);
                tblWorkOrder.ActualDuration = entity.ActualDuration;
                //tblWorkOrder.Deadline = entity.Deadline;
                tblWorkOrder.PlanDuration = entity.PlanDuration;
                tblWorkOrder.PlanStart = entity.PlanStart;
                //tblWorkOrder.ProductId = entity.ProductId;
                tblWorkOrder.Quantity = entity.Quantity;
                tblWorkOrder.QuantityPlanned = entity.QuantityPlanned;
                tblWorkOrder.QuantityProduced = entity.QuantityProduced;
                tblWorkOrder.Status = entity.Status;
                tblWorkOrder.Code = entity.Code;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }

        public List<tblWorkOrder> listAll()
        {
            return db.tblWorkOrders.ToList() ;
        }
        public List<tblWorkOrder> listAllByFilter(int LineId, int Status, DateTime startDate, DateTime endDate)
        {
            return db.tblWorkOrders.Where(p =>
            (p.LineCode == db.tblLines.FirstOrDefault(x => x.Id == LineId).Code || LineId == 0)
            && (Convert.ToDateTime(p.PlanStart).Date >= startDate.Date) && (Convert.ToDateTime(p.PlanStart).Date <= endDate.Date)
            && (Convert.ToDateTime(p.PlanFinish).Date >= startDate.Date) && (Convert.ToDateTime(p.PlanFinish).Date <= endDate.Date)
            && (p.Status == Status || Status == -1)).OrderByDescending(p=>p.Id).ToList();
        }
        public List<tblWorkOrder> listAllByWorkOrderPlan(string Code)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                return db.tblWorkOrders.Where(x => x.Code.StartsWith(Code) && x.Status != STATUS3 && (x.QuantityPlanned == null || x.QuantityPlanned < x.Quantity)).ToList();
            }
            else
            {
                return db.tblWorkOrders.Where(x => x.Status != STATUS3 && (x.QuantityPlanned == null || x.QuantityPlanned < x.Quantity)).ToList();
            }

        }

        

        public tblWorkOrder ViewDetail(long id)
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
        public tblWorkOrder ViewDetailForWorkOrderCode(string code)
        {
            try
            {
                return db.tblWorkOrders.SingleOrDefault(x => x.Code == code);
            }
            catch
            {
                return null;
            }
        }
        public tblWorkOrderPlan ViewDetailWorkPlanId(long id)
        {
            try
            {
                return db.tblWorkOrderPlans.FirstOrDefault(x => x.WorkPlanId == id);
            }
            catch
            {
                return null;
            }
        }

        public tblWorkOrder GetWorkOrderId(long id)
        {
            var wop = db.tblWorkOrderPlans.SingleOrDefault(x => x.WorkPlanId == id);
            if (wop != null)
            {
                var wo = db.tblWorkOrders.SingleOrDefault(x => x.Code == wop.WorkOrderCode);
                return wo;
            }
            else
            {
                return null;
            }
            
        }
        public int CheckExistCode(int Id, string Code)
        {
            try
            {
                return db.tblWorkOrders.Where(x => (Id == 0 || x.Id != Id) && Code.Equals(x.Code)).Count();
            }
            catch
            {
                return 0;
            }
        }
        public void UpdateQuantity(long id, int QuantityPlanned, DateTime StartTime, DateTime FinishTime, bool isUpdateTime, bool isAdd)
        {
            tblWorkOrder wO = db.tblWorkOrders.SingleOrDefault(x => x.Id == id);
            if (wO.QuantityPlanned == null)
            {
                wO.QuantityPlanned = 0;
            }
            if (isAdd)
            {
                wO.QuantityPlanned = wO.QuantityPlanned + QuantityPlanned;
            }
            else
            {
                wO.QuantityPlanned = wO.QuantityPlanned - QuantityPlanned;
            }

            if (wO.QuantityPlanned > wO.Quantity)
            {
                wO.QuantityPlanned = wO.Quantity;
            }
            if (wO.QuantityPlanned < 0)
            {
                wO.QuantityPlanned = 0;
            }
            if (isUpdateTime)
            {
                wO.PlanStart = StartTime;
                wO.PlanFinish = FinishTime;
            }
            db.SubmitChanges();
        }
        public bool Delete(long id)
        {
            try
            {
                var WorkOrder = db.tblWorkOrders.SingleOrDefault(x => x.Id == id);
                db.tblWorkOrders.DeleteOnSubmit(WorkOrder);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
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
            else if (Status == STATUS4)
            {
                status = "Quá hạn";
            }
            return status;
        }
    }
}
