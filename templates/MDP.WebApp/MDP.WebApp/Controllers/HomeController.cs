using System;
using Microsoft.AspNetCore.Mvc;

namespace MDP.WebApp
{
    public class HomeController : Controller
    {
        // Fields
        private readonly WorkService _workService = null;


        // Constructors
        public HomeController(WorkService workService)
        {
            // Default
            _workService = workService;
        }


        // Methods
        public ActionResult Index()
        {
            // ViewBag
            this.ViewBag.Message = _workService.GetValue();

            // Return
            return View();
        }
    }
}
