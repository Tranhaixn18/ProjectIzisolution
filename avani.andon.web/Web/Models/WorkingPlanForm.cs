using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class WorkingPlanForm 
    {
        public int WorkingId { get; set; }
        public string ShiftIds { get; set; }
        public string ShiftNames { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Days { get; set; }
        public string NodeIds { get; set; }
        public string LineIds { get; set; }
        public string NodeNames { get; set; }
        public string LineNames { get; set; }
        public int Plan { get; set; }
        public int HeadCount { get; set; }
        public int TaktTime { get; set; }
        public double Quality { get; set; }

        public int StartHour { get; set; }
        public int B1StartHour { get; set; }
        public int B2StartHour { get; set; }
        public int B3StartHour { get; set; }
        public int StartMinute { get; set; }
        public int B1StartMinute { get; set; }
        public int B2StartMinute { get; set; }
        public int B3StartMinute { get; set; }
        public int FinishHour { get; set; }
        public int B1FinishHour { get; set; }
        public int B2FinishHour { get; set; }
        public int B3FinishHour { get; set; }
        public int FinishMinute { get; set; }
        public int B1FinishMinute { get; set; }
        public int B2FinishMinute { get; set; }
        public int B3FinishMinute { get; set; }
        public int Duration { get; set; }
        public int B1Duration { get; set; }
        public int B2Duration { get; set; }
        public int B3Duration { get; set; }
        public int BreakDuration { get; set; }
        public int WorkingDuration { get; set; }
        /*public void Cast(int iYear, int iMonth, int iWorkingId)
        {
            int Year = iYear, Month = iMonth;
            int headCount = 0,taktTime = 0,plan = 0;
            double quality = 0;
            int StartHour = 0, B1StartHour = 0, B2StartHour = 0, B3StartHour = 0, FinishHour = 0, B1FinishHour = 0, B2FinishHour = 0, B3FinishHour = 0;
            int StartMinute = 0, B1StartMinute = 0, B2StartMinute = 0, B3StartMinute = 0, FinishMinute = 0, B1FinishMinute = 0, B2FinishMinute = 0, B3FinishMinute = 0;
            int Duration = 0, B1Duration = 0, B2Duration = 0, B3Duration = 0, BreakDuration = 0, WorkingDuration = 0;
            List<tblWorkPlan> plans = new WorkingPlanDao().ListAll(iWorkingId);
            string strDays = "; ", strLineIds = "; ", strLineNames = "; ", strShiftIds = "; ", strShiftNames = "; ", strNodeTypes = "; ";
            foreach (tblWorkPlan p in plans)
            {
                if (!strDays.Contains("; " + p.Day + ";"))
                {
                    strDays += p.Day + "; ";
                }
                if (!strLineIds.Contains("; " + p.LineId + ";"))
                {
                    strLineIds += p.LineId + "; ";
                    strLineNames += new LineDao().GettblLineNameById(p.LineId) + "; ";
                }

                if (!strShiftIds.Contains("; " + p.ShiftId + ";"))
                {
                    strShiftIds += p.ShiftId + "; ";
                    strShiftNames += new WorkingShiftDao().GetNameById(p.ShiftId) + "; ";
                }
                Year = p.Year;
                Month = p.Month;
                headCount = p.HeadCount;
                taktTime = p.TaktTime;
                quality = p.Quality;

                StartHour = p.StartHour;
                B1StartHour = p.B1StartHour==null?0:Convert.ToInt32(B1StartHour);
                B2StartHour = p.B2StartHour == null ? 0 : Convert.ToInt32(B2StartHour);
                B3StartHour = p.B3StartHour == null ? 0 : Convert.ToInt32(B3StartHour);

                StartMinute = p.StartMinute;
                B1StartMinute = p.B1StartMinute == null ? 0 : Convert.ToInt32(B1StartMinute);
                B2StartMinute = p.B2StartMinute == null ? 0 : Convert.ToInt32(B2StartMinute);
                B3StartMinute = p.B3StartMinute == null ? 0 : Convert.ToInt32(B3StartMinute);

                FinishHour = p.FinishHour;
                B1FinishHour = p.B1FinishHour == null ? 0 : Convert.ToInt32(B1FinishHour);
                B2FinishHour = p.B2FinishHour == null ? 0 : Convert.ToInt32(B2FinishHour);
                B3FinishHour = p.B3FinishHour == null ? 0 : Convert.ToInt32(B3FinishHour);

                FinishMinute = p.FinishMinute;
                B1FinishMinute = p.B1FinishMinute == null ? 0 : Convert.ToInt32(B1FinishMinute);
                B2FinishMinute = p.B2FinishMinute == null ? 0 : Convert.ToInt32(B2FinishMinute);
                B3FinishMinute = p.B3FinishMinute == null ? 0 : Convert.ToInt32(B3FinishMinute);


                Duration = p.Duration;
                B1Duration = p.B1Duration == null ? 0 : Convert.ToInt32(B1Duration);
                B2Duration = p.B2Duration == null ? 0 : Convert.ToInt32(B3Duration);
                B3Duration = p.B3Duration == null ? 0 : Convert.ToInt32(B3Duration);



            }
            this.WorkingId = iWorkingId;
            this.Year = Year;
            this.Month = Month;

            if (strShiftIds.Length > 2)
            {
                strShiftIds = strShiftIds.Substring(2);
            }
            if (strShiftNames.Length > 2)
            {
                strShiftNames = strShiftNames.Substring(2);
            }
            if (strDays.Length > 2)
            {
                strDays = strDays.Substring(2);
            }
            if (strLineIds.Length > 2)
            {
                strLineIds = strLineIds.Substring(2);
            }
            if (strLineNames.Length > 2)
            {
                strLineNames = strLineNames.Substring(2);
            }
            if (strNodeTypes.Length > 2)
            {
                strNodeTypes = strNodeTypes.Substring(2);
            }

            this.ShiftIds = strShiftIds;
            this.ShiftNames = strShiftNames;
            this.Days = strDays;
            this.LineIds = strLineIds;
            this.LineNames = strLineNames;
            this.Plan = plan;
            this.HeadCount = headCount ;
              this.TaktTime = taktTime;
              this.Quality = quality;

            this.StartHour = StartHour;
            this.B1StartHour = B1StartHour;
            this.B2StartHour = B2StartHour;
            this.B3StartHour = B3StartHour;
            this.StartMinute = StartMinute;
            this.B1StartMinute = B1StartMinute;
            this.B2StartMinute = B2StartMinute;
            this.B3StartMinute = B3StartMinute;

            this.FinishHour = FinishHour;
            this.B1FinishHour = B1FinishHour;
            this.B2FinishHour = B2FinishHour;
            this.B3FinishHour = B3FinishHour;
            this.FinishMinute = FinishMinute;
            this.B1FinishMinute = B1FinishMinute;
            this.B2FinishMinute = B2FinishMinute;
            this.B3FinishMinute = B3FinishMinute;
        }*/

        public bool isShow(int Shift, string LineId)
        {
            bool ret = true;
            
            if (ret)
            {
                if (Shift != 0)
                {
                    ret = ("; " + this.ShiftIds).Contains("; " + Shift + ";");
                }
            }

            if (ret)
            {
                if (LineId.Trim() != "")
                {
                    ret = this.LineNames.Contains(LineId.ToUpper());
                }
            }
            return ret;

        }

    }
}