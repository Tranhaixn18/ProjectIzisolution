using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class WorkPlanIndex: tblWorkPlan
    {
        public string LineName { get; set; }
        public string ProductionName { get; set; }
    }
}
