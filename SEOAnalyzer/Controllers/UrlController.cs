using System.Web.Mvc;

namespace SEOAnalyzer.Controllers
{
    public class UrlController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Url - SEO Analyzer";

            return View();
        }
    }
}
