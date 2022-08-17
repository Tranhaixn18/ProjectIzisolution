using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ProductionQualityReportModel
    {
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string WorkOrderCode { get; set; }
        public string ProductionName { get; set; }
        public string ProductCode { get; set; }
        public string Model { get; set; }
        public string ProductName { get; set; }
        public int? ActualQuantiity { get; set; }
        public int? FailQuantiity { get; set; }
        public int? PassQuantiity { get; set; }
        public decimal Perfromance { get; set; }
        public decimal PerfromancePercent { get; set; }
    }
}
