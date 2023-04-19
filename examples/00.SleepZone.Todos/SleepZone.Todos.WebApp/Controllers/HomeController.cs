using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepZone.Todos.WebApp.Controllers
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
