using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly DemoService _demoService;


        // Constructors
        public HomeController(DemoService demoService)
        {
            // Default
            _demoService = demoService;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _demoService.GetMessage();

            // Return
            return View();
        }
    }
}
