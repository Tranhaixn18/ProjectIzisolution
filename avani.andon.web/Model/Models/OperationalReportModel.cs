using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class OperationalReportModel
    {
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string NodeName { get; set; }
        public string WorkPlanHour { get; set; }
        public string RunTimeHour { get; set; }
        public string StopTimeHour { get; set; }
        public string DisconnectTimeHour { get; set; }
        public string EffectiveOperatingRate { get; set; }
    }
}
