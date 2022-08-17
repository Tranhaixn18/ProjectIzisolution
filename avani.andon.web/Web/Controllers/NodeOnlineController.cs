using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.DataModel;
using System.Configuration;
using System.Globalization;
using System.Threading;
using avSVAW.Models;

namespace avSVAW.Controllers
{
    public class NodeOnlineController : Controller
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);

        // GET: EventDef
        /// <summary>  
        /// First Action method called when page loads  
        /// Fetch all the rows from DB and display it  
        /// </summary>  
        /// <returns>Home View</returns>  
        public ActionResult Index(string id)
        {
            tblLine n = new tblLine();
            string NodeName = "";
            if (!string.IsNullOrEmpty(id))
            {
                n = new LineDao().ViewDetail(Convert.ToInt32(id));
                if (n != null)
                {
                    NodeName = n.Code;
                }
            }
            //List<tblWorkOrder> lstWorkOrder = new WorkOrderDao().listAll();
            //List<WorkOrderForm> models = new List<WorkOrderForm>();
            //foreach(tblWorkOrder wO in lstWorkOrder)
            //{
            //    WorkOrderForm wOF = new WorkOrderForm();
            //    wOF.Cast(wO);
            //    models.Add(wOF);
            //}
            //ViewBag.ListWorkOrder = models;
            ViewBag.NodeName = NodeName;
            ViewBag.NodeId = id;
            ViewBag.EventDef = new EventDefDao().listAll();
            ViewBag.TimeNow = DateTime.Now.ToString("HH:mm:ss");
            return View("Index");
        }

        /// <summary>  
        /// Action method, called when the "Add New Record" link clicked  
        /// </summary>  
        /// <returns>Create View</returns>  
        /*[HttpPost]*/
        /*public ActionResult UpdateStatus(string Id,string WorkPlanId)
        {
            int iId = 0;
            int NodeId = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                try
                {
                    iId = Convert.ToInt32(Id);
                    var daoWOD = new WorkOrderPlanDao();
                    tblWorkOrderPlan orderPlan = daoWOD.ViewDetail(iId);
                    //NodeId = Convert.ToInt32(orderPlan.NodeId);
                    tblWorkOrder WorkOrder = new WorkOrderDao().ViewDetail(Convert.ToInt32(orderPlan.WorkOrderId));
                    //orderPlan.Quantity = orderPlan.Quantity == null ? 0 : orderPlan.Quantity;
                   // orderPlan.QuantityProduced = orderPlan.QuantityProduced == null ? 0 : orderPlan.QuantityProduced;
                   // int iQuantity = Convert.ToInt32(orderPlan.Quantity) - Convert.ToInt32(orderPlan.QuantityProduced);
                    List<tblWorkOrderPlan> lstWOP = daoWOD.getByWorkPlanId(Convert.ToInt32(orderPlan.WorkPlanId));

                    //WorkOrder.QuantityPlanned = WorkOrder.QuantityPlanned + iQuantity;
                    new WorkOrderDao().Update(WorkOrder);

                    daoWOD.Update(orderPlan);

                    int lastIndex = lstWOP.FindLastIndex(x => x.Id == iId);
                    int endIndex = lstWOP.Count - 1;
                    tblWorkOrderPlan lastOrderPlan = new tblWorkOrderPlan();
                    tblWorkOrderPlan endOrderPlan = lstWOP[endIndex];
                    if (lastIndex > 0) {
                         lastOrderPlan = lstWOP[lastIndex];
                    }
                    if (endOrderPlan.Id != orderPlan.Id)
                    {
                        lastOrderPlan.NextId = endIndex;
                        endOrderPlan.NextId = orderPlan.Id;
                        orderPlan.NextId = null;
                        orderPlan.nOrder = endOrderPlan.nOrder + 1;
                        *//*if(orderPlan.QuantityProduced>= orderPlan.Quantity)
                        {
                            orderPlan.Status = WorkOrderPlanDao.STATUS2;
                        }
                        else
                        {
                            orderPlan.Status = WorkOrderPlanDao.STATUS3;
                        }*//*
                       

                        daoWOD.UpdateEntity(lastOrderPlan);
                        daoWOD.UpdateEntity(endOrderPlan);
                        daoWOD.UpdateEntity(orderPlan);
                    }
                }
                catch
                {
                    
                }
            }
          
            return RedirectToAction("Index", "NodeOnline", new { NodeId = NodeId });
        }*/

       
        
        
        
        
        
    }
}