using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class ListLossForm //: tblNodeEvent
    {
       public int LineId { get; set; }
       public string LineName { get; set; }
       public string NodeName { get; set; }
       public string TT1 { get; set; }
       public string TT2 { get; set; }
       public string TT3 { get; set; }
       public string TT4 { get; set; }

       public string ProblemEN { get; set; }
       public string ProblemVN { get; set; }
       public string Color { get; set; }
        public string CallOn { get; set; }
        public string Response { get; set; }
       public string Resolved { get; set; }
       public string TotalDuration { get; set; }
       public double TotalTime { get; set; }

        public string ConvertMin2Time(double TotalMinute)
        {
            string ret = "";
            long _hour = (long)TotalMinute / 60;
            long _min = (long)TotalMinute % 60;

            ret += (_hour < 10 ? "0" + _hour.ToString() : _hour.ToString());
            ret += ":" + (_min < 10 ? "0" + _min.ToString() : _min.ToString());

            return ret;
        }
        /*public void Cast(tblNodeEvent NodeEvent)
        {
            if(NodeEvent.NodeId > 0)
            {
                int NodeId = Convert.ToInt32(NodeEvent.NodeId);
                tblNode n = new NodeDao().ViewDetail(NodeId);
                int LineId = 0;
                string LineName = "";
                if (n.LineId > 0)
                {
                    LineId = Convert.ToInt32(n.LineId);
                    tblLine l = new LineDao().ViewDetail(LineId);
                    LineName = l.Name;
                }
                //if(NodeEvent.T4!=null && NodeEvent.T3 != null)
                //{
                //    double TotalSecond = (Convert.ToDateTime(NodeEvent.T3) - Convert.ToDateTime(NodeEvent.T4)).TotalMinutes;
                //    this.CallOn = ConvertMin2Time(TotalSecond);
                //}
                if(NodeEvent.T2!=null && NodeEvent.T3 != null)
                {
                    double TotalSecond = (Convert.ToDateTime(NodeEvent.T2) - Convert.ToDateTime(NodeEvent.T3)).TotalMinutes;
                    this.Response = ConvertMin2Time(TotalSecond);
                }
                if (NodeEvent.T1!=null && NodeEvent.T2 != null)
                {
                    double TotalSecond = (Convert.ToDateTime(NodeEvent.T1) - Convert.ToDateTime(NodeEvent.T2)).TotalMinutes;
                    this.Resolved = ConvertMin2Time(TotalSecond);
                }
                if (NodeEvent.T1!=null && NodeEvent.T3 != null)
                {
                    double TotalSecond = (Convert.ToDateTime(NodeEvent.T1) - Convert.ToDateTime(NodeEvent.T3)).TotalMinutes;
                    this.TotalTime = TotalSecond;
                    this.TotalDuration = ConvertMin2Time(TotalSecond);
                }
                tblEventDef eDef = new EventDefDao().ViewDetail(Convert.ToInt32(NodeEvent.EventDefId));
                if (eDef != null)
                {
                    this.ProblemEN = eDef.Name_EN;
                    this.ProblemVN = eDef.Name_VN;
                    this.Color = eDef.Color;
                }

                this.Id = NodeEvent.Id;
                this.NodeId = NodeEvent.NodeId;
                this.NodeName = n.Name;
                this.T1 = NodeEvent.T1;
                this.T2 = NodeEvent.T2;
                this.T3 = NodeEvent.T3;
                //this.T4 = NodeEvent.T4;
                this.TT1 = NodeEvent.T1!=null ? Convert.ToDateTime(NodeEvent.T1).ToString("yyyy-MM-dd HH:mm:ss"):"";
                this.TT2 = NodeEvent.T2 != null ? Convert.ToDateTime(NodeEvent.T2).ToString("yyyy-MM-dd HH:mm:ss") : "";
                this.TT3 = NodeEvent.T3 != null ? Convert.ToDateTime(NodeEvent.T3).ToString("yyyy-MM-dd HH:mm:ss") : "";
                //this.TT4 = NodeEvent.T4 != null ? Convert.ToDateTime(NodeEvent.T4).ToString("yyyy-MM-dd HH:mm:ss") : "";
                this.EventDefId = NodeEvent.EventDefId;
                this.LineId = LineId;
                this.LineName = LineName;
                this.Reason = NodeEvent.Reason==null?"": NodeEvent.Reason;
            }
           
        }*/
       
    }
    public class GroupReasonForm
    {
        public string GroupName { get; set; }
        public List<tblEventReason> Items { get; set; }
    }
}