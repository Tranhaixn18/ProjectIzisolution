using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class WorkOrderPlanForm : tblWorkOrderPlan
    {
        public tblProduct Product { get; set; }
        public tblWorkOrder WorkOrder { get; set; }
        public tblNode Node { get; set; }
        public int RemainQuantity { get; set; }
        public string PlanStartStr { get; set; }
        public string PlanFinishStr { get; set; }
        public string StatusStr { get; set; }

        public void Update()
        {
            WorkOrderPlanDao nodeDao = new WorkOrderPlanDao();
            nodeDao.Update(Cast());
        }

        public void Cast(tblWorkOrderPlan node)
        {
            //tblWorkOrder _workOrder = new WorkOrderDao().ViewDetail(Convert.ToInt64(node.WorkOrderId));
            tblWorkOrder _workOrder = new WorkOrderDao().ViewDetailForWorkOrderCode(node.WorkOrderCode);
            this.RemainQuantity = 0;
            if (_workOrder != null)
            {
                tblProduct prod = new ProductDao().ViewDetail(_workOrder.ProductCode);
                this.Product = prod;
                this.WorkOrder = _workOrder;
                if (_workOrder.Quantity != null && _workOrder.QuantityPlanned != null)
                {
                    this.RemainQuantity = Convert.ToInt32(_workOrder.Quantity) - Convert.ToInt32(_workOrder.QuantityPlanned);
                }
            }
            else
            {
                this.WorkOrder = new tblWorkOrder();
                this.Product = new tblProduct();
            }
            this.Id = node.Id;
            /*this.NodeId = node.NodeId;
            if (node.NodeId != null) {
                this.Node = new NodeDao().ViewDetail(Convert.ToInt32(node.NodeId));
            }*/

            this.ActualTaktTime = node.ActualTaktTime;
            //this.Duration = node.Duration;
            this.Finished = node.Finished;
            this.HeadCount = node.HeadCount;
            this.NextId = node.NextId;
            this.nOrder = node.nOrder;
            this.PlanFinish = node.PlanFinish;
            this.PlanStart = node.PlanStart;
            this.PlanStartStr = node.PlanStart == null ? "" : Convert.ToDateTime(node.PlanStart).ToString("yyyy/MM/dd HH:mm:ss");
            this.PlanFinishStr = node.PlanFinish == null ? "" : Convert.ToDateTime(node.PlanFinish).ToString("yyyy/MM/dd HH:mm:ss");
            //this.Quantity = node.Quantity; 
            //this.QuantityProduced = node.QuantityProduced;
            this.Started = node.Started;
            this.Status = node.Status;
            this.TaktTime = node.TaktTime;
            this.UPH = node.UPH;
            this.UPPH = node.UPPH;
            //this.WorkOrderId = node.WorkOrderId;
            this.WorkPlanId = node.WorkPlanId;
            this.ProductCode = node.ProductCode;
            this.ProductName = node.ProductName;
            this.WorkOrderCode = node.WorkOrderCode;
            this.ProductionName = node.ProductionName;
            this.StatusStr = new WorkOrderPlanDao().getStatusName(Convert.ToByte(this.Status));


        }
        public tblWorkOrderPlan Cast()
        {
            return new tblWorkOrderPlan()
            {
                Id = this.Id,
                nOrder = this.nOrder,
                ActualTaktTime = this.ActualTaktTime,
                //Duration = this.Duration,
                Finished = this.Finished,
                HeadCount = this.HeadCount,
                NextId = this.NextId,
                PlanFinish = this.PlanFinish,
                PlanStart = this.PlanStart,
                //Quantity = this.Quantity,
                //QuantityProduced = this.QuantityProduced,
                PlanQuantity = this.PlanQuantity,
                Started = this.Started,
                Status = this.Status,
                TaktTime = TaktTime,
                UPH = this.UPH,
                UPPH = this.UPPH,
                //WorkOrderId = this.WorkOrderId,
                WorkPlanId = this.WorkPlanId
            };
        }
    }
}