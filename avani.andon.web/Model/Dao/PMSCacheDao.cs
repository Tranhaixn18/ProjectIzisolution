using Model.DataModel;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class PMSCacheDao
    {
        AvaniDataContext db = null;
        public PMSCacheDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            }else
            {
                db = new AvaniDataContext();
            }
        }

        /*public List<tblPMSCache> listAll()
        {
            List<tblPMSCache> lst = new List<tblPMSCache>();
            lst = db.tblPMSCaches.ToList();
            return lst.OrderBy(x=>x.LineId).ToList();
        }

        public List<tblPMSCache> listByLineAndLogsTime(int LineId,DateTime LogsTime)
        {
            List<tblPMSCache> lst = new List<tblPMSCache>();
            lst = db.tblPMSCaches.Where(p=>p.LineId==LineId && p.LogsTime!=null && Convert.ToDateTime(p.LogsTime) == LogsTime).ToList();
            return lst.OrderBy(x => x.LogsTime).ToList();
        }
        public List<tblPMSCache> listByLineAndWorkingId(int LineId, long WorkingId )
        {
            List<tblPMSCache> lst = new List<tblPMSCache>();
            lst = db.tblPMSCaches.Where(p => p.LineId == LineId && p.WorkingPlanId == WorkingId).ToList();
            return lst.OrderBy(x => x.LogsTime).ToList();
        }
        public tblPMSCache ViewDetail(long id)
        {
            try
            {
                return db.tblPMSCaches.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }*/
    }

}
