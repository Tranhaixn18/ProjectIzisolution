using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.DataModel;
using System.Configuration;
using System.Threading;
using System.Globalization;
using RabbitMQ.Client;
using Newtonsoft.Json;
using avSVAW.Models;
using System.Text;

namespace avSVAW.Controllers
{
    public class LineOnlineController : BaseController
    {
        string strConStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        public int Hour2UpdateReportDaily = int.Parse(ConfigurationManager.AppSettings["UpdateReportDaily"]);
        private string _CMSRabbitMQHost = ConfigurationManager.AppSettings["RabbitMQ.CMS.Host"];
        private int _CMSRabbitMQPort = int.Parse(ConfigurationManager.AppSettings["RabbitMQ.CMS.Port"]);
        private string _CMSRabbitMQVirtualHost = ConfigurationManager.AppSettings["RabbitMQ.CMS.VirtualHost"];
        private string _CMSRabbitMQUser = ConfigurationManager.AppSettings["RabbitMQ.CMS.User"];
        private string _CMSRabbitMQPassword = ConfigurationManager.AppSettings["RabbitMQ.CMS.Password"];
        private string _CMSRoutingKey = ConfigurationManager.AppSettings["RabbitMQ.CMS.Routing"];
        private string _CMSExchange = ConfigurationManager.AppSettings["RabbitMQ.CMS.Exchange"];
        private static ConnectionFactory cmsConnectionFactory = new ConnectionFactory();
        private static IConnection cmsConnection;
        private static bool _isCMSConnection;
        private static IModel cmsSubcriber;

        public ActionResult Index(string LineId)
        {

            var sessionLang = Session[GlobalConstants.LANG_SESSION];
            string culture = "vi-VN";
            if (sessionLang != null)
            {
                string lang = Convert.ToString(sessionLang);
                culture = string.Empty;
                if (lang.ToLower().CompareTo("vi") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "vi-VN";
                }
                if (lang.ToLower().CompareTo("en") == 0 || string.IsNullOrEmpty(culture))
                {
                    culture = "en-US";
                }

            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            List<tblEventDef> EventDef = new EventDefDao().listAll();
            ViewBag.EventDefs = EventDef;
            string LineName = "";
            tblLine l = new tblLine();
            List<tblNode> lstNode = new List<tblNode>();
            if (!string.IsNullOrEmpty(LineId))
            {
                l = new LineDao().ViewDetail(Convert.ToInt32(LineId));
                if (l != null)
                {
                    LineName = l.Name;
                    lstNode = new NodeDao().ListtblNodeByLineId(l.Id);
                }
            }
            ViewBag.LineId = LineId;
            ViewBag.LineName = LineName;
            ViewBag.lstNode = lstNode;
            string color1 = "", color2 = "", color3 = "", num1 = "", num2 = "", num3 = "";

            if (
                ConfigurationManager.AppSettings["NUM_1"] != null &&
                ConfigurationManager.AppSettings["NUM_2"] != null &&
                ConfigurationManager.AppSettings["NUM_3"] != null &&
                ConfigurationManager.AppSettings["COLOR_1"] != null &&
                ConfigurationManager.AppSettings["COLOR_2"] != null &&
                ConfigurationManager.AppSettings["COLOR_3"] != null
                )
            {
                num1 = ConfigurationManager.AppSettings["NUM_1"].ToString();
                num2 = ConfigurationManager.AppSettings["NUM_2"].ToString();
                num3 = ConfigurationManager.AppSettings["NUM_3"].ToString();
                color1 = ConfigurationManager.AppSettings["COLOR_1"].ToString();
                color2 = ConfigurationManager.AppSettings["COLOR_2"].ToString();
                color3 = ConfigurationManager.AppSettings["COLOR_3"].ToString();
            }
            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Num3 = num3;
            ViewBag.Color1 = color1;
            ViewBag.Color2 = color2;
            ViewBag.Color3 = color3;

            DateTime dt = DateTime.Now;
            ViewBag.TimeNow = dt.ToString("HH:mm:ss");

            return View("Index");
        }

        public void GetNodeLine(string NodeLine, out int iNodeLine)
        {
            iNodeLine = 0;
            if (!string.IsNullOrEmpty(NodeLine))
            {
                iNodeLine = Convert.ToInt32(NodeLine);
            }
            ViewBag.LineId = NodeLine;
            List<SelectListItem> Lines = new List<SelectListItem>();
            List<tblLine> lst = new LineDao().listAll();
            foreach (var nt in lst)
            {
                Lines.Add(new SelectListItem() { Value = nt.Id.ToString(), Text = nt.Name.ToString() });
            }

            Lines.Insert(0, new SelectListItem { Text = "-- " + Resources.Language.All + " --", Value = "" });

            ViewBag.Lines = Lines;
        }

        [HttpGet]
        public ActionResult LineMornitoring(string lineId, string Status, string startDate, string endDate)
        {
            int iLineId = 0;
            GetNodeLine(lineId, out iLineId);
            ViewBag.LineId = lineId;
            if (!string.IsNullOrEmpty(lineId))
            {
                iLineId = Convert.ToInt32(lineId);
            }
            DateTime fromDate = DateTime.Now;
            var nowDate = DateTime.Now;
            fromDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                fromDate = Convert.ToDateTime(startDate);
            }
            ViewBag.StartDate = fromDate.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(endDate))
            {
                toDate = Convert.ToDateTime(endDate);
            }
            ViewBag.EndDate = toDate.ToString("yyyy/MM/dd");
            int iStatus = -1;
            if (!string.IsNullOrEmpty(Status))
            {
                iStatus = Convert.ToInt32(Status);
            }
            List<SelectListItem> listStatus = new List<SelectListItem>();
            for (int i = -1; i < 6; i++)
            {
                if (i == -1) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "-- " + @Resources.Language.All + "--" });
                else if (i == 0) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Kế hoạch" });
                else if (i == 1) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Đang chạy" });
                else if (i == 2) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Tạm dừng" });
                else if (i == 3) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Hoàn thành" });
                else if (i == 4) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Quá hạn" });
                else if (i == 5) listStatus.Add(new SelectListItem() { Value = i.ToString(), Text = "Hủy bỏ" });
            }
            ViewBag.Status = listStatus;
            ViewBag.Statuss = iStatus;


            var result = new LineOnlineDao().GetLineMornitor(iLineId,iStatus, fromDate, toDate);
            return View("LineMornitoring", result);
        }


        //Send to RabbitMQ

        [HttpGet]
        public ActionResult SendStatusForRun(int id)
        {
            cmsConnectionFactory.UserName = _CMSRabbitMQUser;
            cmsConnectionFactory.Password = _CMSRabbitMQPassword;
            cmsConnectionFactory.VirtualHost = _CMSRabbitMQVirtualHost;
            cmsConnectionFactory.HostName = _CMSRabbitMQHost;
            cmsConnectionFactory.Port = _CMSRabbitMQPort;
            using (cmsConnection = cmsConnectionFactory.CreateConnection())
            using (var cmsPublisher = cmsConnection.CreateModel())
            {
                //cmsPublisher.ExchangeDeclare(exchange: _CMSExchange, type: ExchangeType.Topic, durable: true, false, null);
                //cmsPublisher.QueueDeclare(_CMSRoutingKey, true, false, false, null);
                var wop = new WorkOrderPlanDao().ViewDetail2(id);
                //new WorkOrderPlanDao().UpdateStatusToStop(wop);
                string message = "{'message_type': 'processing','data':'" + JsonConvert.SerializeObject(wop) + "'}";
                var body = Encoding.UTF8.GetBytes(message);
                cmsPublisher.BasicPublish(exchange: _CMSExchange, routingKey: _CMSRoutingKey, basicProperties: null, body: body);
                Thread.Sleep(5000);
                return RedirectToAction("LineMornitoring", "LineOnline");
            }
        }

        [HttpGet]
        public ActionResult SendStatusForStop(int id)
        {
            cmsConnectionFactory.UserName = _CMSRabbitMQUser;
            cmsConnectionFactory.Password = _CMSRabbitMQPassword;
            cmsConnectionFactory.VirtualHost = _CMSRabbitMQVirtualHost;
            cmsConnectionFactory.HostName = _CMSRabbitMQHost;
            cmsConnectionFactory.Port = _CMSRabbitMQPort;
            using (cmsConnection = cmsConnectionFactory.CreateConnection())
            using (var cmsPublisher = cmsConnection.CreateModel())
            {
                //cmsPublisher.ExchangeDeclare(exchange: _CMSExchange, type: ExchangeType.Topic, durable: true, false, null);
                //cmsPublisher.QueueDeclare(_CMSRoutingKey, true, false, false, null);
                var wop = new WorkOrderPlanDao().ViewDetail2(id);
                //new WorkOrderPlanDao().UpdateStatusToRun(wop);
                string message = "{'message_type': 'pause','data':'" + JsonConvert.SerializeObject(wop) + "'}";
                var body = Encoding.UTF8.GetBytes(message);
                cmsPublisher.BasicPublish(exchange: _CMSExchange, routingKey: _CMSRoutingKey, basicProperties: null, body: body);
                Thread.Sleep(5000);
                return RedirectToAction("LineMornitoring", "LineOnline");

            }
        }

        [HttpGet]
        public ActionResult SendStatusForComplete(int id)
        {
            cmsConnectionFactory.UserName = _CMSRabbitMQUser;
            cmsConnectionFactory.Password = _CMSRabbitMQPassword;
            cmsConnectionFactory.VirtualHost = _CMSRabbitMQVirtualHost;
            cmsConnectionFactory.HostName = _CMSRabbitMQHost;
            cmsConnectionFactory.Port = _CMSRabbitMQPort;
            using (cmsConnection = cmsConnectionFactory.CreateConnection())
            {


                using (var cmsPublisher = cmsConnection.CreateModel())
                {
                    //cmsPublisher.ExchangeDeclare(exchange: _CMSExchange, type: ExchangeType.Topic, durable: true, false, null);
                    cmsPublisher.QueueDeclare(_CMSRoutingKey, true, false, false, null);
                    cmsPublisher.QueueBind(_CMSRoutingKey, _CMSExchange, _CMSRoutingKey);
                    var wop = new WorkOrderPlanDao().ViewDetail2(id);
                    //new WorkOrderPlanDao().UpdateStatusToRun(wop);
                    string message = "{'message_type': 'cancel','data':'" + JsonConvert.SerializeObject(wop) + "'}";
                    var body = Encoding.UTF8.GetBytes(message);
                    cmsPublisher.BasicPublish(exchange: _CMSExchange, routingKey: _CMSRoutingKey, basicProperties: null, body: body);
                    Thread.Sleep(5000);
                    return RedirectToAction("LineMornitoring", "LineOnline");

                }
            }
        }
    }
}