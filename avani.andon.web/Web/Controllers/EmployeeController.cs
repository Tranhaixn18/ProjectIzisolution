using avSVAW.Models;
using Model.Dao;
using Model.DataModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Web.Mvc;

namespace avSVAW.Controllers
{
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<EmployeeForm> model = new List<EmployeeForm>();
            List<tblEmployee> list = new EmployeeDao().GetAll();
            foreach (tblEmployee item in list)
            {
                EmployeeForm empl = new EmployeeForm();
                empl.Cast(item);
                model.Add(empl);
            }
            
            return View(model);
        }
        [HttpGet]
        public ActionResult Received()
        {
            
            return RedirectToAction("Index");
        }
        public EmployeeForm EditOrCreate(tblEmployee request = null)
        {
            EmployeeForm empl = new EmployeeForm();
            if (request != null)
            {
                empl = new EmployeeForm().GetEmplForm(request);
            }
            //else
            //{
            //    empl.Active = true;
            //}

            List<tblDepartment> listDept = new DepartDao().listAll();
            empl.Depts = new List<SelectListItem>();
            empl.Depts.Add(new SelectListItem()
            {
                Value = "",
                Text = "Chọn"
            });

            for (int i = 0; i < listDept.Count; i++)
            {
                empl.Depts.Add(new SelectListItem()
                {
                    Value = listDept[i].Id.ToString(),
                    Text = listDept[i].Name
                });
            }


            return empl;
        }
        [HttpGet]
        public ActionResult Create()
        {
            EmployeeForm empl = EditOrCreate(null);
            //var factory = new ConnectionFactory()
            //{
            //    HostName = "localhost",
            //    Port = 5672,
            //    UserName = "guest",
            //    Password = "guest"
            //};
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{

            //    channel.QueueDeclare(queue: "demo",
            //                     durable: false,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);
            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (Model, ea) =>
            //    {
            //        var body = ea.Body.ToArray();
            //        var message = JsonConvert.SerializeObject(Encoding.UTF8.GetString(body));
            //        //var jsonMess = JsonConvert.DeserializeObject(message);
            //        ViewBag.MessageRabbit = message;
            //    };

            //    channel.BasicConsume(queue: "demo",
            //                 noAck: false,
            //                 consumer: consumer);
            //}
            return View(empl);
        }
        [HttpPost]
        public ActionResult Create(EmployeeForm request)
        {
            if (ModelState.IsValid)
            {
                tblEmployee empl = new EmployeeForm().GetBaseEmpl(request);
                var dao = new EmployeeDao();
                int id = dao.Insert(empl);
                if (id == 0)
                {

                    ViewBag.Failure = 0;
                    EmployeeForm model = EditOrCreate(request);
                    return View(model);
                }
                if (id == -1)
                {
                    ViewBag.Failure = -1;
                    EmployeeForm model = EditOrCreate(request);
                    return View(model);
                }
                if (id == -2)
                {
                    ViewBag.Failure = -2;
                    EmployeeForm model = EditOrCreate(request);
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var emplObj = new EmployeeDao().ViewDetail(Convert.ToInt32(id));
            EmployeeForm model = EditOrCreate(emplObj);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(EmployeeForm request)
        {
            if (ModelState.IsValid)
            {
                tblEmployee emplObj = new EmployeeForm().GetBaseEmpl(request);
                var emplDao = new EmployeeDao();
                var result = emplDao.Update(emplObj);

                if (result == 0)
                {
                    ViewBag.Failure = 0;
                    EmployeeForm model1 = EditOrCreate(request);
                    return View(model1);
                }
                else if (result == -1)
                {
                    ViewBag.Failure = -1;
                    EmployeeForm model1 = EditOrCreate(request);
                    return View(model1);
                }
                else
                {
                    return RedirectToAction("Index", "Employee");
                }
            }
            EmployeeForm model = EditOrCreate(request);
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            new EmployeeDao().Delete(id);
            return RedirectToAction("Index", "Employee");
        }

        public ActionResult Rabiit()
        {
            return View();
        }
    }
}