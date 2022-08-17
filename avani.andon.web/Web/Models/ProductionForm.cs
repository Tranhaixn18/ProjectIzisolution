using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class pmscaches /*: tblPMSCache*/
    {
        public string strLogsTime { get; set; }
        /*public void cast()
        {
            if (this.LogsTime != null)
            {
                this.strLogsTime =Convert.ToDateTime(this.LogsTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                this.strLogsTime = "";
            }
        }*/
    }
    public class ProductionForm 
    {
        public List<pmscaches> pmsCache { get; set; }

        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string FinishHour { get; set; }
        public string FinishMinute { get; set; }
      
        public string StartTime { get; set; }
        public string StopTime { get; set; }
    }
}