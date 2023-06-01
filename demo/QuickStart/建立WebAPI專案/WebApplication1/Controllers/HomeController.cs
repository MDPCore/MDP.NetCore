using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult<object> Index()
        {
            // Message
            var message = "Hello World";

            // Return
            return new { message = message };
        }
    }
}
