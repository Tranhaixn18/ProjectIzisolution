using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class LineScheduleForm:tblLineSchedule
    {
        public void Cast(tblLineSchedule n)
        {
            this.Id = n.Id;
            this.LineCode = n.LineCode;
            this.LineName = n.LineName;
            this.ScheduleCode = n.ScheduleCode;
            this.ScheduleName = n.ScheduleName;
        }
        public tblLineSchedule Cast()
        {
            return new tblLineSchedule()
            {
                Id = this.Id,
                LineCode = this.LineCode,
                LineName = this.LineName,
                ScheduleCode = this.ScheduleCode,
                ScheduleName = this.ScheduleName
            };
        }
    }
}