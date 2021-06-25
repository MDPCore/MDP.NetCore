using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.WebApp
{
    public class HomeController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult Index()
        {
            // Return
            return View();
        }
    }
}
