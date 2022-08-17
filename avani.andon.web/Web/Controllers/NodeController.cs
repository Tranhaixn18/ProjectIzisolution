using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using Model.Dao;
using Model.DataModel;
using avSVAW.Models;

namespace avSVAW.Controllers
{
    public class NodeController : BaseController
    {
        public ActionResult Index(string LineId)
        {
            ViewBag.Factories = new FactoryDao().listAll();
           
            ViewBag.Zones = new ZoneDao().listAll();
            //ViewBag.NodeType = new NodeTypeDao().listAll();
            int iLineId = 0;
            if (!string.IsNullOrEmpty(LineId))
            {
                iLineId = Convert.ToInt32(LineId);
            }
            ViewBag.Lines = new LineDao().listAll();
            ViewBag.Line = LineId;
            List<tblNode> lst = new NodeDao().listAll().Where(p=>p.LineId==iLineId || iLineId==0).OrderBy(x => x.LineId).ThenBy(x => x.nOrder).ToList();
            List<NodeForm> model = new List<NodeForm>();
            foreach(tblNode l in lst)
            {
                NodeForm lf = new NodeForm();
                lf.Cast(l);
                model.Add(lf);
            }
            return View("Index", model);
        }
  
        public ActionResult Create()
        {
            NodeForm model = new NodeForm();
            model.Lines = new LineDao().listAll().Select(n =>
                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Code
                }).ToList();

            //model.Zones = new ZoneDao().listAll().Select(n =>
            //    new SelectListItem
            //    {
            //        Value = n.Id.ToString(),
            //        Text = n.Name
            //    }).ToList();

            //model.NodeTypes = new NodeTypeDao().listAll().Select(n =>
            //    new SelectListItem
            //    {
            //        Value = n.Id.ToString(),
            //        Text = n.Code
            //    }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NodeForm model)
        {
            if (ModelState.IsValid)
            {
                tblNode l = model.Cast();
                new NodeDao().Insert(l);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            tblNode l = new NodeDao().ViewDetail(Id);
            NodeForm model = new NodeForm();
            model.Cast(l);
            model.Lines = new LineDao().listAll().Select(n =>
                   new SelectListItem
                   {
                       Value = n.Id.ToString(),
                       Text = n.Code
                   }).ToList();

            //model.Zones = new ZoneDao().listAll().Select(n =>
            //    new SelectListItem
            //    {
            //        Value = n.Id.ToString(),
            //        Text = n.Name
            //    }).ToList();

            //model.NodeTypes = new NodeTypeDao().listAll().Select(n =>
            //    new SelectListItem
            //    {
            //        Value = n.Id.ToString(),
            //        Text = n.Code
            //    }).ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(NodeForm model)
        {
            tblNode l = model.Cast();
            new NodeDao().Update(l);
            // udpate NodeONline
           return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            new NodeDao().Delete(Id);
            return RedirectToAction("Index");
        }


        [WebMethod]
        public JsonResult GetNodeJsonList()
        {
            List<tblNode> lst = new NodeDao().listAll();
            List<NodeForm> model = new List<NodeForm>();
            foreach (tblNode l in lst)
            {
                NodeForm lf = new NodeForm();
                lf.Cast(l);
                model.Add(lf);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}