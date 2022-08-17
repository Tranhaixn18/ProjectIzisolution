using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class DetailOperationReportModel
    {
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string NodeName { get; set; }
        public string EventDefName { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public DateTime? Sub { get; set; }
        public TimeSpan ProcessDuration { get; set; }
    }
}
