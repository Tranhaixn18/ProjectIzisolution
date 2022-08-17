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
    public class LogForm /*: tblLog*/
    {
        public long Id { get; set; }
        public int NodeId { get; set; }
        public double Temp { get; set; }
        public double Humd{ get; set; }
        public DateTime UpdateTime { get; set; }
        public string strTime { get; set; }

        /*public void Cast(tblLog node)
        {
            this.Id = node.Id;
            //this.NodeId = node.NodeId;
            this.UpdateTime = (DateTime)node.UpdateTime;
            this.Temp = node.Temp==null?0:Convert.ToDouble(node.Temp);
            this.Humd = node.Humd == null?0:Convert.ToDouble(node.Humd);
            this.strTime = this.UpdateTime.ToString("HH:mm");
        }*/

        public void Cast(DataRow dr)
        {

            this.Id = (dr["Id"] == DBNull.Value ? 0 : (int)dr["Id"]);
            this.NodeId = (dr["NodeId"] == DBNull.Value ? 0 : (int)dr["NodeId"]);
            this.UpdateTime = (dr["UpdateTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["UpdateTime"];
            this.Temp = (dr["Temp"] == DBNull.Value ? 0 : (float)dr["Temp"]);
            this.Humd = (dr["Humd"] == DBNull.Value ? 0 : (float)dr["Humd"]);


        } 
    }
}