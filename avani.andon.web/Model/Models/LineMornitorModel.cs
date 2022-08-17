using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class LineMornitorModel
    {
        public long Id { get; set; }
        public string LineName { get; set; }
        public string LineCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductionName { get; set; }
        public string ImageUrl { get; set; }
        public int? PlanQuantity { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public string WorkOrderCode { get; set; }
        public bool IsAuto { get; set; }
        public int? Status { get; set; }
    }
}
