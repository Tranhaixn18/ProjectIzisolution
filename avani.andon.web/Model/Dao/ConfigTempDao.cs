using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ConfigTempDao
    {
        AvaniDataContext db = null;
        public ConfigTempDao()
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

        public long Insert(tblConfigTemp entity)
        {
            try
            {
                db.tblConfigTemps.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception e) { }
            return entity.Id;
        }
        

        public List<tblConfigTemp> listAll()
        {
            return db.tblConfigTemps.ToList();
        }
        public tblConfigTemp ViewDetail(int id)
        {
            try
            {
                return db.tblConfigTemps.SingleOrDefault(x => x.Id == id);
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
                var line = db.tblConfigTemps.SingleOrDefault(x => x.Id == id);
                db.tblConfigTemps.DeleteOnSubmit(line);
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
