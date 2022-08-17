using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class RabbitModel
    {
        public string message_type { get; set; }
        public tblWorkOrderPlan data { get; set; }
    }
}