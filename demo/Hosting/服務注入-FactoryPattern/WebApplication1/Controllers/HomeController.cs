using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly DemoContext _demoContext;


        // Constructors
        public HomeController(DemoContext demoContext)
        {
            // Default
            _demoContext = demoContext;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _demoContext.GetMessage();

            // Return
            return View();
        }
    }
}
