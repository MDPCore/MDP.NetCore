using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.AspNetCore;
using System.Data;
using Microsoft.Extensions.Logging;

namespace MDP.AspNetCore.Lab
{
    public class HomeController : Controller
    {
        // Fields
        private WorkService _workService = null;


        // Constructors
        public HomeController(WorkService workService)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException(nameof(workService));

            #endregion

            // Default
            _workService = workService; 
        }


        // Methods
        public ActionResult<string> Index()
        {
            // Message
            var message = _workService.GetValue();
            if (string.IsNullOrEmpty(message) == true) throw new InvalidOperationException($"{nameof(message)}=null");

            // ViewBag
            this.ViewBag.Message = message;

            // Return
            return View();
        }
    }
}
