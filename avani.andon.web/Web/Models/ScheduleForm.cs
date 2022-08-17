using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class ScheduleForm:tblSchedule
    {
        public void Cast(tblSchedule n)
        {
            this.Id = n.Id;
            this.Name = n.Name;
            this.FromDate = n.FromDate;
            this.ToDate = n.ToDate;
            this.StartHour = n.StartHour;
            this.StartMinute = n.StartMinute;
            this.FinishHour = n.FinishHour;
            this.FinishMinute = n.FinishMinute;
            this.TotalMinute = n.TotalMinute;
            this.DayOfWeek = n.DayOfWeek;
            this.DayPeriod = n.DayPeriod;
            this.GroupCode = n.GroupCode;
        }
        public tblSchedule Cast()
        {
            return new tblSchedule()
            {
                Id = this.Id,
                Name = this.Name,
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                StartHour = this.StartHour,
                StartMinute = this.StartMinute,
                FinishHour = this.FinishHour,
                FinishMinute = this.FinishMinute,
                TotalMinute = this.TotalMinute,
                DayOfWeek = this.DayOfWeek,
                DayPeriod = this.DayPeriod,
                GroupCode = this.GroupCode,
            };
        }
    }
}