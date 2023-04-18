using ASPNETWebMVCBasic.BLL;
using ASPNETWebMVCBasic.ViewModels;
using System.Web.Mvc;
using System.Web.Security;

namespace ASPNETWebMVCBasic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}