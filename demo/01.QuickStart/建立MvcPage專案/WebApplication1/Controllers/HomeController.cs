using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = "Hello World";

            // Return
            return View();
        }
    }
}
