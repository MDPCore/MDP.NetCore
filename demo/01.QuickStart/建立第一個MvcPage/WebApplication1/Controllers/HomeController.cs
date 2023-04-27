using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult Index()
        {
            return View();
        }
    }
}
