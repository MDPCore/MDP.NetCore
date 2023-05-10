using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly UserContext _userContext;


        // Constructors
        public HomeController(UserContext userContext)
        {
            // Default
            _userContext = userContext;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _userContext.Find()?.Name;

            // Return
            return View();
        }
    }
}
