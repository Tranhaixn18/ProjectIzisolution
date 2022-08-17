using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class EventReasonDao
    {
        AvaniDataContext db = null;
        public EventReasonDao()
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


        public long Insert(tblEventReason entity)
        {
            try
            {
                db.tblEventReasons.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception ex){ }
            return entity.Id;
        }
        public bool Update(tblEventReason entity)
        {
            try
            {
                var reason = db.tblEventReasons.SingleOrDefault(x => x.Id == entity.Id);

                reason.Description = entity.Description;
                reason.GroupId = entity.GroupId;
                reason.GroupName = entity.GroupName;
                reason.nOrder = entity.nOrder;
                reason.Name = entity.Name;

                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblEventReason> listAll()
        {
           
                return db.tblEventReasons.OrderBy(x=>x.nOrder).ToList();
        } 
        public List<tblEventReason> listByFilter(int groupId)
        {
           
                return db.tblEventReasons.Where(x=>x.GroupId == groupId).ToList();
        }
        public List<tblEventReason> listByFilterGroup(int Id)
        {

            return db.tblEventReasons.Where(x =>(x.Id != Id || Id==0) && x.GroupId==0).ToList();
        }
        public tblEventReason ViewDetail(long id)
        {
            try
            {
                return db.tblEventReasons.SingleOrDefault(x => x.Id == id);
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
                var product = db.tblEventReasons.SingleOrDefault(x => x.Id == id);
                db.tblEventReasons.DeleteOnSubmit(product);
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
