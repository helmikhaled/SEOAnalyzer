using System.Web.Mvc;

namespace SEOAnalyzer.Controllers
{
    public class PlainTextController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Plain Text - SEO Analyzer";

            return View();
        }
    }
}
