using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class ProductChartForm
    {
        public string Date { get; set; }
        public int Quantity { get; set; }
        public decimal TaktTime { get; set; }
        public int UPH { get; set; }
        public int UPPH { get; set; }
    }
      
}