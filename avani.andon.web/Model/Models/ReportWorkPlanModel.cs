using System;


namespace Model.Models
{
    public class ReportWorkPlanModel
    {
        public string LineName { get; set; }
        public string LineCode { get; set; }
        public string WorkOrderCode { get; set; }
        public string ProductionName { get; set; }
        public string Model { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? Started { get; set; }
        public int? ActualQuantity { get; set; }
        public int? PlanQuantity { get; set; }
        public int? Status { get; set; } // tblWorkOrder
        public decimal? Performance { get; set; }
        public decimal? PerformancePercent { get; set; }
    }
}
