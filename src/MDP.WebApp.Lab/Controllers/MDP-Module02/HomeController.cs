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


        // Constructors
        public HomeController(SettingContext settingContext)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // Default
            _settingContext = settingContext; 
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
