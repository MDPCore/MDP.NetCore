using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.Module01;
using MDP.AspNetCore;
using System.Data;
using Microsoft.Extensions.Logging;

namespace MDP.WebApp.Lab.Temp
{
    [Module("MDP-Module02")]
    public class HomeController : Controller
    {
        // Fields
        private SettingContext _settingContext = null;

        private readonly ILogger _logger;


        // Constructors
        public HomeController(SettingContext settingContext, ILogger<HomeController> logger)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));
            if (logger == null) throw new ArgumentException(nameof(logger));

            #endregion

            // Default
            _settingContext = settingContext; 
            _logger = logger;

            // Logger-Message
            _logger.LogTrace("Trace Message");
            _logger.LogDebug("Debug Message");
            _logger.LogInformation("Information Message");
            _logger.LogWarning("Warning Message");
            _logger.LogError("Error Message");
            _logger.LogCritical("Critical Message");
        }


        // Methods
        public ActionResult<string> Index()
        {
            // Message
            var message = _settingContext.GetValue();
            if (string.IsNullOrEmpty(message) == true) throw new InvalidOperationException($"{nameof(message)}=null");

            // ViewBag
            this.ViewBag.Message = message;

            // Return
            return View();
        }
    }
}
