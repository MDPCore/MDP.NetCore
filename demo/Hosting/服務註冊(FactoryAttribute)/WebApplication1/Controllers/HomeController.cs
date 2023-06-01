using MDP.Logging;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly ILogger<HomeController> _logger;


        // Constructors
        public HomeController(ILogger<HomeController> logger)
        {
            // Default
            _logger = logger;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = "Hello World";

            // Log
            _logger.LogError(this.ViewBag.Message);

            // Return
            return View();
        }
    }
}
