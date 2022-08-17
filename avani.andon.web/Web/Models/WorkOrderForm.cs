using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class WorkOrderForm : tblWorkOrder
    {
        public List<SelectListItem> Products { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LineName { get; set; }
        public string StatusName { get; set; }
        public double TaktTime { get; set; }
        public int Speed { get; set; }
        public long Create()
        {
            WorkOrderDao nodeDao = new WorkOrderDao();
            return nodeDao.Insert(Cast());
        }

        public void Update()
        {
            WorkOrderDao nodeDao = new WorkOrderDao();
            nodeDao.Update(Cast());
        }

        public void Cast(tblWorkOrder node)
        {
            this.Id = node.Id;
            this.Code = node.Code;
            this.ActualDuration = node.ActualDuration;
            //this.Deadline = node.Deadline;
            this.PlanDuration = node.PlanDuration;
            this.PlanStart = node.PlanStart;
            this.PlanFinish = node.PlanFinish;
            this.ProductionName = node.ProductionName;
            //this.ProductId = node.ProductId;
            this.ProductCode = node.ProductCode;
            this.LineName = new LineDao().ViewDetailForCode(node.LineCode).Name;
            tblProduct p = new ProductDao().ViewDetail(node.ProductCode);
            if (p != null)
            {
                this.ProductName = p.Name;
                this.ProductCode = p.Code;
                this.Speed = p.Speed != null ? Convert.ToInt32(p.Speed) : 0;
                this.TaktTime = p.TaktTime != null ? Convert.ToDouble(p.TaktTime) : 0;
            }
            else
            {
                this.ProductName = "";
                this.ProductCode = "";
                this.TaktTime = 0;
                this.Speed = 0;
            }

            this.Quantity = node.Quantity;
            this.QuantityPlanned = node.QuantityPlanned;
            this.QuantityProduced = node.QuantityProduced;
            this.Status = node.Status;


            if (node.Status != null)
            {
                this.StatusName = new WorkOrderDao().getStatusName(Convert.ToByte(node.Status));
            }
            else
            {
                this.StatusName = "";
            }
            
        }
        public tblWorkOrder Cast()
        {
            return new tblWorkOrder()
            {
                Id = this.Id,
                Code = this.Code,
                ActualDuration = this.ActualDuration,
                //Deadline = this.Deadline,
                PlanDuration = this.PlanDuration,
                PlanStart = this.PlanStart,
                PlanFinish = this.PlanFinish,
                //ProductId = this.ProductId,
                Quantity = this.Quantity,
                QuantityPlanned = this.QuantityPlanned,
                QuantityProduced = this.QuantityProduced,
                Status = 0,
            };
        }

        public tblWorkOrder GetBaseForm(WorkOrderForm request)
        {
            tblWorkOrder wo = new tblWorkOrder();
            //wo.Id = request.Id;
            //wo.LineCode = request.LineCode;
            //wo.Code = request.Code;
            //wo.ProductId = request.ProductId;
            //wo.ProductCode = request.ProductCode;
            //wo.Quantity = request.Quantity;
            //wo.Deadline = request.Deadline;
            //wo.PlanStart = request.PlanStart;
            //wo.PlanFinish = request.PlanFinish;
            //wo.PlanDuration = request.PlanDuration;
            //wo.ActualDuration = request.ActualDuration;
            //wo.QuantityPlanned = request.QuantityPlanned;
            //wo.QuantityProduced = request.QuantityProduced;
            wo.Status = request.Status;
            return wo;
        }
    }
}