using Model.DataModel;
using Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class NodeOnlineDao
    {
        AvaniDataContext db = null;
        public NodeOnlineDao()
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
        /*public bool UpdateNodeName(int NodeId, string NodeName) {
            try
            {
                var tblNodeOnline = db.tblNodeOnlines.Where(x => x.NodeId == NodeId).FirstOrDefault();
                if (tblNodeOnline != null)
                {
                    tblNodeOnline.NodeName = NodeName;
                    db.SubmitChanges();
                }
                else
                {
                    return false;
                }
                   
                
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }
        public bool UpdateTimeOut(int iNodeTypeId, int iTimeOut)
        {
            try
            {
                var tblNodeOnline = db.tblNodeOnlines.Where(x => x.NodeTypeId == iNodeTypeId).ToList();
                foreach (var item in tblNodeOnline)
                {
                    item.TimeOut = iTimeOut;
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblNodeOnline> listAll(bool RemoveNotExist = false)
        {
            List<tblNodeOnline> lst = new List<tblNodeOnline>();
            if (!RemoveNotExist)
            {
                lst = db.tblNodeOnlines.ToList();
            }
            else
            {
                lst = db.tblNodeOnlines.Where(x => x.NodeName != "NC").ToList();
            }

            return lst.OrderBy(x=>x.LineId).ThenBy(x => x.ZoneId).ThenBy(x => x.nOrder).ToList();
        }
        public List<NodeOnlineForm> listAllByJoin(bool RemoveNotExist = false)
        {
            List<NodeOnlineForm> lst = new List<NodeOnlineForm>();
            if (!RemoveNotExist)
            {
                lst = (from n in db.tblNodes
                             join no in db.tblNodeOnlines on n.Id equals no.NodeId into INode
                       from iNo in INode.DefaultIfEmpty()
                       where n.Name != "NC"
                       orderby n.nOrder 
                             select new NodeOnlineForm()
                             {
                                 NodeId = n.Id,
                                 NodeName = n.Name,
                                 LineId = iNo.LineId,
                                 LineName = iNo.LineName,
                                 ZoneId = iNo.ZoneId,
                                 ZoneName = iNo.ZoneName,
                                 Active = iNo.Active,
                                 nOrder = iNo.nOrder,
                                 FactoryId = iNo.FactoryId,
                                 FactoryName = iNo.FactoryName,
                                 NodeTypeId = iNo.NodeTypeId,
                                 NodeTypeName = iNo.NodeTypeName,
                                 Status = iNo.Status,
                                 UpdateTime = iNo.UpdateTime,
                                 DataToShow = iNo.DataToShow,
                                 OnlineTimeCount = iNo.OnlineTimeCount,
                                 WorkingTimeCount = iNo.WorkingTimeCount,
                                 TimeOut = iNo.TimeOut,
                                 Planned = iNo.Planned,
                             }).ToList();
            }
            else
            {
                lst = (from n in db.tblNodes
                             join no in db.tblNodeOnlines on n.Id equals no.NodeId into INode
                             from iNo in INode.DefaultIfEmpty()
                             where n.Name != "NC"
                             orderby n.nOrder
                             select new NodeOnlineForm()
                             {
                                 NodeId = n.Id,
                                 NodeName = n.Name,
                                 LineId = iNo.LineId,
                                 LineName = iNo.LineName,
                                 ZoneId = iNo.ZoneId,
                                 ZoneName = iNo.ZoneName,
                                 Active = iNo.Active,
                                 nOrder = iNo.nOrder,
                                 FactoryId = iNo.FactoryId,
                                 FactoryName = iNo.FactoryName,
                                 NodeTypeId = iNo.NodeTypeId,
                                 NodeTypeName = iNo.NodeTypeName,
                                 Status = iNo.Status,
                                 UpdateTime = iNo.UpdateTime,
                                 DataToShow = iNo.DataToShow,
                                 OnlineTimeCount = iNo.OnlineTimeCount,
                                 WorkingTimeCount = iNo.WorkingTimeCount,
                                 TimeOut = iNo.TimeOut,
                                 Planned = iNo.Planned
                             }).ToList();
            }
            

            return lst.ToList();
        }
        public IEnumerable<tblNodeOnline> ListAllPaging(string searchString)
        {
            IQueryable<tblNodeOnline> model = db.tblNodeOnlines;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NodeName.Contains(searchString));
            }

            return model.OrderByDescending(x => x.nOrder).ToList();
        }
        public tblNodeOnline ViewDetail(int id)
        {
            try
            {
                return db.tblNodeOnlines.SingleOrDefault(x => x.NodeId == id);
            }
            catch
            {
                return null;
            }
        }
        public string GetNameById(int id)
        {
            var obj = ViewDetail(id);
            return obj.NodeName;
        }

        public List<tblNodeOnline> ListByTypeId(int TypeId)
        {
            List<tblNodeOnline> model = db.tblNodeOnlines.ToList().Where(x => x.NodeTypeId == TypeId).ToList();

            return model;
        }
        public List<tblNodeOnline> ListByLineId(int LineId)
        {
            List<tblNodeOnline> model = db.tblNodeOnlines.ToList().Where(x => x.LineId == LineId).ToList();

            return model;
        }
        public List<tblNodeOnline> ListZoneId(int ZoneId)
        {
            List<tblNodeOnline> model = db.tblNodeOnlines.ToList().Where(x => x.ZoneId == ZoneId).ToList();

            return model;
        }*/
    }
}
