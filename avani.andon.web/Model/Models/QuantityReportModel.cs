using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class QuantityReportModel
    {
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public long? PlanDuration { get; set; }
        public int? PlanQuantity { get; set; }
        public long? ActualDuration { get; set; }
        public int? ActualQuantity { get; set; }
        public decimal Perfromance { get; set; }
        public decimal PerfromancePercent { get; set; }
        
    }
}
