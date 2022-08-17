using avSVAW.Models;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
namespace avSVAW.Controllers
{
    public class DepartmentController : BaseController
    {
        // GET: Department
        public ActionResult Index()
        {
            List<tblDepartment> model = new DepartmentDAO().GetAll();
            return View("Index", model);
        }

        public ActionResult Create()
        {
            DepartmentForm model = new DepartmentForm();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDepartment model)
        {
            if (ModelState.IsValid)
            {
                //tblDepartment depart = new tblDepartment();
                //depart.Code = model.Code;
                //depart.Name = model.Name;
                //depart.Description = model.Description;
                DepartmentDAO d= new DepartmentDAO();
               
                    if (d.insert(model) == 0)
                    {
                        ViewBag.Failure = 0;
                        DepartmentForm empl = new DepartmentForm();
                        empl.Code = model.Code;
                        empl.Name = model.Name;
                        empl.Description = model.Description;

                        return View(empl);

                    }
                    else if (d.insert(model) == -1)
                    {
                        ViewBag.Failure = -1;
                        DepartmentForm empl = new DepartmentForm();
                        empl.Code = model.Code;
                        empl.Name = model.Name;
                        empl.Description = model.Description;
                        return View(empl);
                    }
                    else if (d.insert(model) == -2)
                    {
                        ViewBag.Failure = -2;
                        DepartmentForm empl = new DepartmentForm();
                        empl.Code = model.Code;
                        empl.Name = model.Name;
                        empl.Description = model.Description;
                        return View(empl);
                    }
                    else if(d.insert(model) != -3)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        d.insert(model);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            tblDepartment l = new DepartmentDAO().ViewDetail(id);
            DepartmentForm model = new DepartmentForm();
            model.Cast(l);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(DepartmentForm model)
        {
            tblDepartment l = model.Cast();
            DepartmentDAO d = new DepartmentDAO();
            var t = d.Update(l);
            if (t == 0)
            {
                ViewBag.Failure = 0;
                DepartmentForm empl = new DepartmentForm();
                empl.Code = model.Code;
                empl.Name = model.Name;
                empl.Description = model.Description;
                return View(empl);
            }
            if (t == -1)
            {
                ViewBag.Failure = -1;
                DepartmentForm empl = new DepartmentForm();
                empl.Code = model.Code;
                empl.Name = model.Name;
                empl.Description = model.Description;
                return View(empl);
            }
            if (t == -2)
            {
                ViewBag.Failure = -2;
                DepartmentForm empl = new DepartmentForm();
                empl.Code = model.Code;
                empl.Name = model.Name;
                empl.Description = model.Description;
                return View(empl);
            }
            else
                d.Update(l);
            return RedirectToAction("Index");
        }
       
       
        public ActionResult Delete(int id)
        {
            
            DepartmentDAO d = new DepartmentDAO();

            if (d.Delete(id))
            {
                return Redirect("/Department/Index?Delete=success");
            }
                return Redirect("/Department/Index?Delete=false");
        }
        public ActionResult Detail(int id)
        {
            DepartmentDAO d = new DepartmentDAO();
            tblDepartment depart= d.ViewDetail(id);
            ViewBag.TenPhong = depart.Name;
            List<tblEmployee> model = d.ListEmployeeByDepartmentID(id);
            return View("Detail", model);
        }
    }
}