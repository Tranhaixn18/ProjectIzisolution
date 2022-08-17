using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class LogDao
    {
        AvaniDataContext db = null;
        public LogDao()
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
        /*public List<tblLog> listAll()
        {
            List<tblLog> lst = new List<tblLog>();
         
                lst = db.tblLogs.ToList();
         
            return lst.OrderBy(x=>x.UpdateTime).ToList();
        }*/
        /*public List<tblLog> listByFilter(DateTime fromDate, DateTime toDate, int NodeId = 0)
        {
            List<tblLog> lst = new List<tblLog>();
            lst = db.tblLogs.Where(p => p.NodeId == NodeId && Convert.ToDateTime(p.UpdateTime)>=fromDate && Convert.ToDateTime(p.UpdateTime) <=toDate).ToList();
            
            return lst.OrderBy(x => x.UpdateTime).ToList();
        }

        public tblLog ViewDetail(int id)
        {
            try
            {
                return db.tblLogs.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public List<tblLog> ListByNodeId(int NodeId)
        {
            List<tblLog> model = db.tblLogs.ToList().Where(x => x.NodeId == NodeId).ToList();

            return model;
        }*/
    }
}
