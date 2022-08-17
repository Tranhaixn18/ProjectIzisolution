using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class WorkPlanForm : tblWorkPlan
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Days { get; set; }
        public string NodeNames { get; set; }
        public string LineNames { get; set; }
        public int WorkOrderQuantity { get; set; }
        public string WorkOrderCode { get; set; }
        public string PlanFinishStr { get; set; }
        public string PlanStartStr { get; set; }
        public void Cast(tblWorkPlan workPlan)
        {
            this.Id = workPlan.Id;
            this.Status = workPlan.Status;
            this.PlanHeadCount = workPlan.PlanHeadCount;
            this.LineId = workPlan.LineId;
            this.PlanBreakDuration = workPlan.PlanBreakDuration;
            //this.PlanBreakFinish1 = workPlan.PlanBreakFinish1;
            //this.PlanBreakFinish2 = workPlan.PlanBreakFinish2;
            //this.PlanBreakFinish3 = workPlan.PlanBreakFinish3;
            //this.PlanBreakStart1 = workPlan.PlanBreakStart1;
            // this.PlanBreakStart2 = workPlan.PlanBreakStart2;
            // this.PlanBreakStart3 = workPlan.PlanBreakStart3;
            this.PlanFinish = workPlan.PlanFinish;
            this.PlanStart = workPlan.PlanStart;
            this.Takttime = workPlan.Takttime;

            this.PlanStartStr = workPlan.PlanStart == null ? "" : Convert.ToDateTime(workPlan.PlanStart).ToString("yyyy/MM/dd HH:mm:ss");
            this.PlanFinishStr = workPlan.PlanStart == null ? "" : Convert.ToDateTime(workPlan.PlanFinish).ToString("yyyy/MM/dd HH:mm:ss");
            if (workPlan.PlanStart != null)
            {
                DateTime d = Convert.ToDateTime(workPlan.PlanStart);
                this.Year = d.Year;
                this.Month = d.Month;
                //  this.Days = d.ToString("Y-m;
            }
            this.PlanTotalDuration = workPlan.PlanTotalDuration;
            this.PlanWorkingDuration = workPlan.PlanWorkingDuration;
            this.Priority = workPlan.Priority;
            //this.EmployeeId = workPlan.EmployeeId;

            if (workPlan.LineId != null)
            {
                this.LineNames = new LineDao().ViewDetail(Convert.ToInt32(workPlan.LineId)).Name;
                this.LineCode = new LineDao().ViewDetail(Convert.ToInt32(workPlan.LineId)).Code;
            }

            if ((new WorkOrderPlanDao().ViewDetail(workPlan.Id)) != null)
            {
                this.WorkOrderCode = new WorkOrderPlanDao().ViewDetail(workPlan.Id).ProductionName;
            }
            else
            {
                this.WorkOrderCode = null;
            }
            if(new WorkOrderPlanDao().getByWorkPlanId(workPlan.Id) != null)
            {
                this.WorkOrderQuantity = new WorkOrderPlanDao().getByWorkPlanId(workPlan.Id).Count;
            }
            else
            {
                this.WorkOrderQuantity = 0;
            }

        }
        public tblWorkPlan Cast()
        {
            return new tblWorkPlan()
            {
                Id = this.Id,
                Status = this.Status,
                PlanHeadCount = this.PlanHeadCount,
                LineId = this.LineId,
                PlanBreakDuration = this.PlanBreakDuration,
                //PlanBreakFinish1 = this.PlanBreakFinish1,
                //PlanBreakFinish2 = this.PlanBreakFinish2,
                // PlanBreakFinish3 = this.PlanBreakFinish3,
                // PlanBreakStart1 = this.PlanBreakStart1,
                //  PlanBreakStart2 = this.PlanBreakStart2,
                //   PlanBreakStart3 = this.PlanBreakStart3,
                PlanFinish = this.PlanFinish,
                PlanStart = this.PlanStart,
                PlanTotalDuration = this.PlanTotalDuration,
                PlanWorkingDuration = this.PlanWorkingDuration,
                //EmployeeId = this.EmployeeId,
                Priority = this.Priority

            };
        }

    }
}