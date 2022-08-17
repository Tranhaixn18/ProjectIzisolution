using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class OperationForm
    {
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int NodeTypeId { get; set; }
        public string NodeTypeName { get; set; }
        public int EventDefId { get; set; }
        public string EventDefName { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public string T3 { get; set; }
        public string T1 { get; set; }
        public double Duration { get; set; }
        public double PlanDuration { get; set; }
        public string strDuration { get; set; }
        public double ProcessDuration { get; set; }
        public double WaitDuration { get; set; }
        public double ActualDuration { get; set; }
    }
}