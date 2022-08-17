using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace avSVAW.Models
{
    public class ReasonForm : tblEventReason
    {
      public List<tblEventReason> Groups { get; set; }
        public long Create()
        {
            EventReasonDao nodeDao = new EventReasonDao();
            return nodeDao.Insert(Cast());
        }

        public void Update()
        {
            EventReasonDao nodeDao = new EventReasonDao();
            nodeDao.Update(Cast());
        }

        public void Cast(tblEventReason node)
        {
            this.Id = node.Id;
            this.Name = node.Name;
            this.Description = node.Description;
            this.GroupId = node.GroupId;
            this.GroupName = node.GroupName;
            this.nOrder = node.nOrder;
        }
        public tblEventReason Cast()
        {
            return new tblEventReason()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                GroupId = this.GroupId==null?0:this.GroupId,
                GroupName = (this.GroupId==null|| this.GroupId==0) ? this.Name : new EventReasonDao().ViewDetail(Convert.ToInt32(this.GroupId)).Name,
                nOrder = this.nOrder,
            };
        }
    }
}