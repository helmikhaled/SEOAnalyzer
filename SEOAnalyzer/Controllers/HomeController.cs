using System.Web.Mvc;

namespace SEOAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home - SEO Analyzer";

            return View();
        }
    }
}
