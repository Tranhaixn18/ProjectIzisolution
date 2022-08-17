using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace avSVAW.Models
{
    public class WorkOrderPlanHub : Hub //Đây là server
    {
        public void WorkOrderPlanBoardcast(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<WorkOrderPlanHub>();
            context.Clients.All.getData(message);
        }
        

    }
}