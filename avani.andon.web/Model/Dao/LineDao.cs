using Common;
using Model.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class LineDao
    {
        AvaniDataContext db = null;
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        public LineDao()
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

        public long Insert(tblLine entity)
        {
            try
            {
                db.tblLines.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception Ex) { }
            return entity.Id;
        }

        public bool Update(tblLine entity)
        {
            try
            {
                var tblLine = db.tblLines.SingleOrDefault(x => x.Id == entity.Id);
                tblLine.Code = entity.Code;
                tblLine.Name = entity.Name;
                tblLine.Description = entity.Description;
                tblLine.FactoryId = entity.FactoryId;
                tblLine.nOrder = entity.nOrder;
                tblLine.IsAuto=entity.IsAuto;
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblLine> listAll()
        {
            return db.tblLines.OrderBy(x => x.nOrder).ToList();
        }
        public IEnumerable<tblLine> ListAllPaging(string searchString)
        {
            IQueryable<tblLine> model = db.tblLines;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Code.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.Name).ToList();
        }

        public tblLine GetByCode(string code)
        {
            return db.tblLines.SingleOrDefault(x => x.Code == code);
        }
        public tblLine ViewDetail(int id)
        {
            try
            {
                return db.tblLines.SingleOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }
        public string ViewDetailSql(string code)
        {
            using (SqlConnection con = new SqlConnection(strConStr))
            {
                string query = "SELECT Name FROM tblLine WHERE Code = '" + code + "'";
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                var result = command.ExecuteScalar().ToString();
                return result;
            }
        }
        public tblLine ViewDetailForCode(string code)
        {
            try
            {
                return db.tblLines.SingleOrDefault(x => x.Code == code);
            }
            catch
            {
                return null;
            }
        }
        public string GettblLineNameById(int? id)
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

        public bool Delete(int id)
        {
            try
            {
                var line = db.tblLines.SingleOrDefault(x => x.Id == id);
                db.tblLines.DeleteOnSubmit(line);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<tblLine> ListAllByCustomer(int CustomerId = 0, int GroupId = 0)
        {
            List<tblLine> retList = new List<tblLine>();

            try
            {
                //Luôn phải lấy CustomerId

                List<tblLine> tblLines = new List<tblLine>();
                tblLines = db.tblLines.OrderBy(z => z.nOrder).ToList();
                if (CustomerId != 0)
                {
                    tblLines = db.tblLines.Where(x => x.CustomerId == CustomerId).ToList();
                }
                //Liệt kê ra toàn bộ

                foreach (tblLine zone in tblLines)
                {
                    if (GroupId == 0)
                    {
                        retList.Add(zone); //Áp dụng phân quyền, liệt kê ra hết
                    }
                    else
                    {
                        List<tblUserPermission> tblUserPermissions = db.tblUserPermissions.Where(x => x.GroupId == GroupId && x.ObjectId == zone.Id && x.ObjectType == GlobalConstants.LINE_OBJECT_TYPE).ToList();

                        if (tblUserPermissions.Count > 0)
                        {
                            bool _view = false;
                            foreach (var per in tblUserPermissions)
                            {
                                _view = (per.View != null && (bool)per.View) ? true : false;

                                if (_view)
                                    break;
                            }

                            /*if (!_view) //Nếu không có quyền thì thử xem có quyền trong Node nào thuộc Zone đó không
                            {
                                List<tblNode> lstNodes = new NodeDao().ListPermissionNode(GroupId, zone.Id);
                                if (lstNodes.Count > 0)
                                    _view = true;

                            }*/

                            if (_view) //Nếu ông có quyền xem mới được vào
                            {
                                retList.Add(zone);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

            }

            return retList;
        }
    }
}
