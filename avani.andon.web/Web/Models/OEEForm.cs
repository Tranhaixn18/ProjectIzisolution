using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class OEEForm 
    {
        public long Id { get; set; }
        public int Plan { get; set; }
        public int Target { get; set; }
        public int Actual { get; set; }
        public int NG { get; set; }
        public int RunningDuration { get; set; }
        public int StopDuration { get; set; }
        public int PlanDuration { get; set; }
        public double Avaibility { get; set; }
        public double Performance { get; set; }
        public double Quality { get; set; }
        public double OEE { get; set; }

        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string FinishHour { get; set; }
        public string FinishMinute { get; set; }
        public int NumberOfStop { get; set; }
        public int TakeTime { get; set; }
        public int HeadCount { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }

    }
}