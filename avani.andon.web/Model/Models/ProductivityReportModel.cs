using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductivityReportModel
    {
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string WorkOrderCode { get; set; }
        public string ProductionName { get; set; }
        public string ProductCode { get; set; }
        public string Model { get; set; }
        public string ProductName { get; set; }
        public int? PlanQuantity { get; set; }
        public long? PlanDuration { get; set; }
        public long? PlanUPH { get; set; }
        public int? PlanHeadCount { get; set; }
        public int? PlanUPPH { get; set; }
        public long? ActualDuration { get; set; }
        public int? ActualQuantity { get; set; }
        public int? ActualHeadCount { get; set; }
        public int? ActualUPH { get; set; }
        public int? ActualUPPH { get; set; }
        
    }
}
