using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepZone.WebApp
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
            // Logger-Message
            _logger.LogTrace("Trace Message");
            _logger.LogDebug("Debug Message");
            _logger.LogInformation("Information Message");
            _logger.LogWarning("Warning Message");
            _logger.LogError("Error Message");
            _logger.LogCritical("Critical Message");

            // Return
            return View();
        }
    }
}
