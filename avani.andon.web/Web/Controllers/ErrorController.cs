using avSVAW.App_Start;
using System.Web.Mvc;

namespace avSVAW.Controllers
{
    [SessionTimeout]
    public class ErrorController : Controller
    {
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
