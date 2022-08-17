using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class ProductForm : tblProduct
    {
      
        public long Create()
        {
            ProductDao nodeDao = new ProductDao();
            return nodeDao.Insert(Cast());
        }

        public void Update()
        {
            ProductDao nodeDao = new ProductDao();
            nodeDao.Update(Cast());
        }

        public void Cast(tblProduct node)
        {
            this.Id = node.Id;
            this.Name = node.Name;
            this.Description = node.Description;
            this.Code = node.Code;
            this.Status = node.Status;
            this.TaktTime = node.TaktTime;
            this.CalculatedTaktTime = node.CalculatedTaktTime;
            this.Unit = node.Unit;
            this.Quantity = node.Quantity;
            this.Speed = node.Speed;
        }
        public tblProduct Cast()
        {
            return new tblProduct()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Code = this.Code,
                CalculatedTaktTime = this.CalculatedTaktTime,
                Status = this.Status,
                Quantity = this.Quantity,
                TaktTime = this.Speed == null ? 0 : Math.Round(60/(double)this.Speed , 2),
                Unit = this.Unit,
                Speed = this.Speed,
            };
        }
    }
}