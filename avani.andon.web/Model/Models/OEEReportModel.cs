using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class OEEReportModel
    {
        public DateTime? Started { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public long? PlanDuration { get; set; }
        public long? ActualDuration { get; set; }
        public decimal PlanPerformance { get; set; }
        public int? PlanQuantity { get; set; }
        public int? ActualQuantity { get; set; }
        public decimal ActualPerformance { get; set; }
        public int? NG { get; set; }
        public int? Pass { get; set; }
        public decimal Ratio { get; set; }
        public decimal OEE { get; set; }
    }
}
