using Common;
using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class NodeDao
    {
        AvaniDataContext db = null;
        public NodeDao()
        {
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            if (!string.IsNullOrEmpty(con))
            {
                db = new AvaniDataContext(con);
            } else
            {
                db = new AvaniDataContext();
            }
        }

        public long Insert(tblNode entity)
        {
            try
            {
                db.tblNodes.InsertOnSubmit(entity);
                db.SubmitChanges();
                //UpdateNodeDef();
            }
            catch (Exception) {
            }
            return entity.Id;
        }

        public bool Update(tblNode entity)
        {
            try
            {
                var tblNode = db.tblNodes.SingleOrDefault(x => x.Id == entity.Id);
                tblNode.Name = entity.Name;
                tblNode.Description = entity.Description;
                tblNode.Active = entity.Active;
                tblNode.LineId = entity.LineId;
               // tblNode.rId = entity.rId;
                tblNode.ZoneId = entity.ZoneId;
                tblNode.NodeTypeId = entity.NodeTypeId;
                tblNode.nOrder = entity.nOrder;
                db.SubmitChanges();
                UpdateNodeDef();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public void UpdateNodeDef()
        {
            using (var cmd = db.Connection.CreateCommand())
            {
                cmd.CommandText = "exec [UpdateNodeDef]";
                var results = cmd.ExecuteReader();
            }
        }
        public List<tblNode> ListAll()
        {
            return db.tblNodes.ToList();
        }
        public List<tblNode> listAll(bool RemoveNotExist = false)
        {
            if (!RemoveNotExist)
            {
                return db.tblNodes.OrderBy(x => x.LineId).ThenBy(x => x.nOrder).ToList();
            }
            else
            {
                return db.tblNodes.Where(x => x.Name != "NC").OrderBy(x=>x.LineId).ThenBy(x => x.nOrder).ToList();
            }
        
        }
        public IEnumerable<tblNode> ListAllPaging(string searchString)
        {
            IQueryable<tblNode> model = db.tblNodes;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.nOrder).ToList();
        }
        public tblNode ViewDetail(int id)
        {
            try
            {
                return db.tblNodes.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public string GettblNodeNameById(int? id)
        {
            string result = "";
            if (id != null)
            {
                var obj = ViewDetail(id.GetValueOrDefault());
                if (obj != null)
                {
                    result = obj.Name ?? "";
                }
            }
            return result;
        }

        public int GetNodeTypeById(int? id)
        {
            int result = 0;
            if (id != null)
            {
                var obj = ViewDetail(id.GetValueOrDefault());
                if (obj != null)
                {
                    result = obj.NodeTypeId ?? 0;
                }
            }
            return result;
        }

        public bool Delete(int id)
        {
            try
            {
                var node = db.tblNodes.SingleOrDefault(x => x.Id == id);
                db.tblNodes.DeleteOnSubmit(node);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<tblNode> ListtblNodeByTypeId(int TypeId)
        {
            List<tblNode> model = db.tblNodes.ToList().Where(x => x.NodeTypeId == TypeId).ToList();

            return model;
        }
        public List<tblNode> ListtblNodeByLineId(int LineId)
        {
            if (LineId == 0)
            {
                List<tblNode> listn=db.tblNodes.ToList();
                return listn;
            }
            else
            {
                List<tblNode> model = db.tblNodes.Where(x => x.LineId == LineId).ToList();
                return model;
            }
            

            
        }
        public List<tblNode> ListtblNodeByZoneId(int ZoneId)
        {
            List<tblNode> model = db.tblNodes.ToList().Where(x => x.ZoneId == ZoneId).ToList();

            return model;
        }

        /*public List<tblNode> ListPermissionNode(int groupId, int zoneId = 0)
        {
            int _ObjectType = GlobalConstants.NODE_OBJECT_TYPE;
            List<tblNode> retList = new List<tblNode>();
            //Liệt kê ra toàn bộ
            int CustomerId = (int)new UserGroupDao().ViewDetail(groupId).CustomerId;
            List<tblNode> tblNodes = new NodeDao().ListAllByCustomerId(CustomerId);

            //Lọc nếu có ZoneId
            if (zoneId != 0)
            {
                tblNodes = tblNodes.Where(n => n.ZoneId == zoneId).ToList();
            }

            try
            {
                foreach (tblNode node in tblNodes)
                {
                    List<tblUserPermission> tblUserPermissions = db.tblUserPermissions.Where(x => x.GroupId == groupId && (x.ObjectId == node.Id && x.ObjectType == GlobalConstants.NODE_OBJECT_TYPE || x.ObjectId == node.ZoneId && x.ObjectType == GlobalConstants.LINE_OBJECT_TYPE)).ToList();

                    if (tblUserPermissions.Count > 0)
                    {
                        bool _view = false;
                        foreach (var per in tblUserPermissions)
                        {
                            _view = (per.View != null && (bool)per.View) ? true : false;

                            if (_view)
                                break;
                        }

                        if (_view) //Nếu ông có quyền xem mới được vào
                        {
                            retList.Add(node);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }

            return retList;
        }*/
        /*public List<tblNode> ListAllByCustomerId(int CustomerId = 0)
        {
            if (CustomerId == 0)
            {
                return db.tblNodes.OrderBy(n => n.nOrder).ToList();
            }
            else
            {
                return db.tblNodes.Where(x => x.CustomerId == CustomerId).OrderBy(n => n.nOrder).ToList();
            }
        }*/
    }
}
