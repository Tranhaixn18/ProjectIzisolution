using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class NodeForm : tblNode
    {
        public List<SelectListItem> Lines { get; set; }
        public string LineName { get; set; }
        public List<SelectListItem> Zones { get; set; }
        public string ZoneName { get; set; }
        public List<SelectListItem> NodeTypes { get; set; }
        public string NodeTypeName { get; set; }
        public long Create()
        {
            NodeDao nodeDao = new NodeDao();
            return nodeDao.Insert(Cast());
        }

        public void Update()
        {
            NodeDao nodeDao = new NodeDao();
            nodeDao.Update(Cast());
        }

        public void Cast(tblNode node)
        {
            this.Id = node.Id;
            this.Name = node.Name;
            this.Description = node.Description;
            this.Active = node.Active;
            this.LineId = node.LineId;
            //this.rId = node.rId;
            this.LineName = new LineDao().GettblLineNameById(node.LineId);
            this.ZoneId = node.ZoneId;
            //this.ZoneName = new ZoneDao().GettblZoneNameById(node.ZoneId);
            this.nOrder = node.nOrder;

        }
        public tblNode Cast()
        {
            return new tblNode()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Active = this.Active,
                LineId = this.LineId,
                //rId=this.rId,
                ZoneId = this.ZoneId,
                //NodeTypeId = this.NodeTypeId,
                nOrder = this.nOrder
            };
        }
        public tblNode GetBaseNode(NodeForm nodeForm)
        {
            tblNode node = new tblNode();
            node.Id = nodeForm.Id;
            node.Name = nodeForm.Name;
            node.Active = nodeForm.Active;
            node.Description = nodeForm.Description;
            node.LineId = nodeForm.LineId;
            node.NodeTypeId = nodeForm.NodeTypeId;
            node.nOrder = nodeForm.nOrder;
            node.ZoneId = nodeForm.ZoneId;
            return node;
        }
    }
    public class NodeEventForm /*: tblNodeEvent*/
    {
        public string strStartDate { get; set; }
        public string strFromDate { get; set; }
        public string NodeName { get; set; }

        /*public void Cast(tblNodeEvent node)
        {
            this.Id = node.Id;
            this.EventDefId = node.EventDefId;
            this.NodeId = node.NodeId;
            this.Reason = node.Reason;
            this.T1 = node.T1;
            this.T3 = node.T3;
            this.NodeName = new NodeDao().ViewDetail(Convert.ToInt32(node.NodeId)).Name;
            if (node.T3 != null)
                this.strStartDate = Convert.ToDateTime(node.T3).ToString("yyyy/MM/dd HH:mm:ss");
            if (node.T1 != null)
                this.strFromDate = Convert.ToDateTime(node.T1).ToString("yyyy/MM/dd  HH:mm:ss");

        }*/
        /*public tblNodeEvent Cast()
        {
            return new tblNodeEvent()
            {
                Id = this.Id,
                EventDefId = this.EventDefId,
                NodeId = this.NodeId,
                Reason = this.Reason,
                T1 = this.T1,
                T3 = this.T3
            };
        }*/
    }
}