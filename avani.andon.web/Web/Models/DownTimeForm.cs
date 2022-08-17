using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class DownTimeForm
    {
        public int Id { get; set; }
        public string LossEN { get; set; }
        public string LossVN { get; set; }
        public string Color { get; set; }
        public List<double> iNode {get;set;}
        public string ConvertMin2Time(double TotalMinute)
        {
            string ret = "";
            long _hour = (long)TotalMinute / 60;
            long _min = (long)TotalMinute % 60;

            ret += (_hour < 10 ? "0" + _hour.ToString() : _hour.ToString());
            ret += ":" + (_min < 10 ? "0" + _min.ToString() : _min.ToString());

            return ret;
        }
        
    }
   
}