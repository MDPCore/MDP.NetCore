using System;
using Microsoft.AspNetCore.Mvc;

namespace MDP.WebApp
{
    public class HomeController : Controller
    {
        // Fields
        private readonly WorkContext _workContext = null;


        // Constructors
        public HomeController(WorkContext workContext)
        {
            // Default
            _workContext = workContext;
        }


        // Methods
        public ActionResult Index()
        {
            // ViewBag
            this.ViewBag.Message = _workContext.GetValue();

            // Return
            return View();
        }
    }
}
