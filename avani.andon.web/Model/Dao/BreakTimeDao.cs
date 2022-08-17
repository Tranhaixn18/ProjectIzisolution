using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Dao
{
    public class BreakTimeDao
    {
        AvaniDataContext db = null;
        public BreakTimeDao()
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

        public long Insert(tblBreakTime entity)
        {
            try
            {
                CalculateTotalTime(ref entity);
                db.tblBreakTimes.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception e) { }
            return entity.Id;
        }

        private void CalculateTotalTime(ref tblBreakTime entity)
        {
            DateTime StartTime = new DateTime(2019, 1, 1, entity.StartHour, entity.StartMinute, 0);
            DateTime FinishTime = new DateTime(2019, 1, 1, entity.FinishHour, entity.FinishMinute, 0);
            if (entity.FinishHour < entity.StartHour)
            {
                FinishTime = new DateTime(2019, 1, 2, entity.FinishHour, entity.FinishMinute, 0);
            }

            entity.TotalMinute = (int) (FinishTime - StartTime).TotalMinutes;
        }

        public bool Update(tblBreakTime entity)
        {
            try
            {
                CalculateTotalTime(ref entity);

                var tblBreakTime = db.tblBreakTimes.SingleOrDefault(x => x.Id == entity.Id);
                tblBreakTime.Name = entity.Name;
                tblBreakTime.StartHour = entity.StartHour;
                tblBreakTime.StartMinute = entity.StartMinute;
                tblBreakTime.FinishHour = entity.FinishHour;
                tblBreakTime.FinishMinute = entity.FinishMinute;
                tblBreakTime.TotalMinute = entity.TotalMinute;

                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblBreakTime> listAll()
        {
            return db.tblBreakTimes.ToList();
        }
     
        public tblBreakTime ViewDetail(int id)
        {
            try
            {
                return db.tblBreakTimes.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public string GetNameById(int id)
        {
            try
            {
                return db.tblBreakTimes.SingleOrDefault(x => x.Id == id).Name;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var line = db.tblBreakTimes.SingleOrDefault(x => x.Id == id);
                db.tblBreakTimes.DeleteOnSubmit(line);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
