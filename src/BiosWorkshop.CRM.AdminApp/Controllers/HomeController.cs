using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiosWorkshop.CRM.AdminApp
{
    public class HomeController : Controller
    {
        // Fields
        private readonly ILogger _logger = null;


        // Constructors
        public HomeController(ILogger<HomeController> logger)
        {
            #region Contracts

            if (logger == null) throw new ArgumentException(nameof(logger));

            #endregion

            // Default
            _logger = logger;
        }


        // Methods
        [Authorize]
        public ActionResult Index()
        {
            // Return
            return View();
        }
    }
}
