using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class NodeOperationReportModel
    {
        public int NodeId { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string NodeName { get; set; }
        public long? WorkPlan { get; set; }
        public double? RunningDuration { get; set; }
        public double? StopDuration { get; set; }
        public long? WaitDuration { get; set; }
        public decimal Performance { get; set; }
        public decimal PerformancePercent { get; set; }
    }
}
