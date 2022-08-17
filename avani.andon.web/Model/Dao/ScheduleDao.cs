using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
   public class ScheduleDao
    {
        AvaniDataContext db = null;
        public ScheduleDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            }
            else
            {
                db = new AvaniDataContext();
            }
        }
        public List<tblSchedule> ListAll()
        {
            return db.tblSchedules.ToList();
        }
    }
}
