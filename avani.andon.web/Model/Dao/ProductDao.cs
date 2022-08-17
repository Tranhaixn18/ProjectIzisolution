using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Configuration;
using Common;
using Model.DataModel;
using Model.Models;

namespace Model.Dao
{
    public class ProductDao
    {
        AvaniDataContext db = null;
        public ProductDao()
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
        public long Insert(tblProduct entity)
        {
            try
            {
                db.tblProducts.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
            }
            return entity.Id;
        }

        public bool Update(tblProduct entity)
        {
            try
            {
                var product = db.tblProducts.SingleOrDefault(x => x.Id == entity.Id);

                product.CalculatedTaktTime = entity.CalculatedTaktTime;
                product.Code = entity.Code;
                product.Description = entity.Description;
                product.Name = entity.Name;
                product.Quantity = entity.Quantity;
                product.Status = entity.Status;
                product.TaktTime = entity.TaktTime;
                product.Unit = entity.Unit;
                product.Speed = entity.Speed;

                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<tblProduct> ListAll()
        {
            return db.tblProducts.ToList();
        }

        public tblProduct ViewDetail(string productCode)
        {
            try
            {
                return db.tblProducts.SingleOrDefault(x => x.Code == productCode);
            }
            catch
            {
                return null;
            }
        }

        public int getIdByProductCode(string Code)
        {
            try
            {
                tblProduct product = db.tblProducts.Where(x => Code.Equals(x.Code)).FirstOrDefault();
                if (product != null)
                {
                    return product.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.tblProducts.SingleOrDefault(x => x.Id == id);
                db.tblProducts.DeleteOnSubmit(product);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public byte getStatus(string strStatus)
        {
            byte status = 1;
            if ("InActive".Equals(strStatus))
            {
                status = 0;
            }
            else if ("Active".Equals(strStatus))
            {
                status = 1;
            }
            else if ("Deleted".Equals(strStatus))
            {
                status = 2;
            }
            return status;

        }
        public int CheckExistCode(int Id, string Code)
        {
            try
            {
                return db.tblProducts.Where(x => (Id == 0 || x.Id != Id) && Code.Equals(x.Code)).Count();
            }
            catch
            {
                return 0;
            }
        }
        public List<QuantityReportModel> ListbyFilterQuantityReport(int lineId, int productId, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join l in db.tblLines on wop.LineId equals l.Id
                        join p in db.tblProducts on wop.ProductCode equals p.Code
                        where (l.Id == lineId || lineId == 0) && (p.Id == productId || productId == 0)
                        && Convert.ToDateTime(wop.PlanStart) >= startDate && Convert.ToDateTime(wop.PlanStart) <= endDate
                        select new { wop, l, p };
            var data = query.Select(x => new QuantityReportModel()
            {
                LineCode = x.wop.LineCode,
                LineName = x.l.Name,
                ProductCode = x.wop.ProductCode,
                ProductName = x.wop.ProductName,
                PlanDuration = x.wop.PlanDuration,
                PlanQuantity = x.wop.PlanQuantity,
                ActualDuration = x.wop.ActualDuration,
                ActualQuantity = x.wop.ActualQuantity,
                Perfromance = (Convert.ToDecimal((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity) / Convert.ToDecimal((x.wop.PlanQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.PlanQuantity)) * 100,
                PerfromancePercent = Convert.ToDecimal((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity) / Convert.ToDecimal((x.wop.PlanQuantity == 0 || x.wop.PlanQuantity == null) ? 1 : x.wop.PlanQuantity)
            }).ToList();
            return data;
        }
        public List<ProductionQualityReportModel> ListbyFilterProductQuantityReport(int lineId, int productId, DateTime startDate, DateTime endDate)
        {
            var query = from wop in db.tblWorkOrderPlans
                        join l in db.tblLines on wop.LineId equals l.Id
                        join p in db.tblProducts on wop.ProductCode equals p.Code
                        where (l.Id == lineId || lineId == 0) && (p.Id == productId || productId == 0)
                        && Convert.ToDateTime(wop.PlanStart) >= startDate && Convert.ToDateTime(wop.PlanStart) <= endDate
                        select new { wop, l, p };
            var data = query.Select(x => new ProductionQualityReportModel()
            {
                LineCode = x.wop.LineCode,
                LineName = x.l.Name,
                ProductionName = x.wop.ProductionName,
                ProductCode = x.wop.ProductCode,
                Model = x.p.Model,
                ProductName = x.wop.ProductName,
                ActualQuantiity = x.wop.ActualQuantity,
                FailQuantiity = x.wop.NG,
                PassQuantiity = Convert.ToInt32((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity) - Convert.ToInt32((x.wop.NG == 0 || x.wop.NG == null) ? 1 : x.wop.NG),
                Perfromance = Convert.ToDecimal((Convert.ToInt32((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity) - Convert.ToInt32((x.wop.NG == 0 || x.wop.NG == null) ? 1 : x.wop.NG)) / Convert.ToDecimal((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity)),
                PerfromancePercent = Convert.ToDecimal((Convert.ToInt32((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity) - Convert.ToInt32((x.wop.NG == 0 || x.wop.NG == null) ? 1 : x.wop.NG)) / Convert.ToDecimal((x.wop.ActualQuantity == 0 || x.wop.ActualQuantity == null) ? 1 : x.wop.ActualQuantity)) * 100
            }).ToList();
            return data;
        }

    }
}
