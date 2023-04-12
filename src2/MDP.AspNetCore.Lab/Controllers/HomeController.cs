using MDP.Logging;
using MDP.Tracing;
using Microsoft.AspNetCore.Mvc;
using MyLab.Modules;
using System;

namespace MDP.AspNetCore.Lab
{
    public class HomeController : Controller
    {
        // Fields
        private readonly WorkService _workService;


        // Constructors
        public HomeController(WorkService workService, ILogger<Program> logger, ITracer<Program> tracer)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException($"{nameof(workService)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");
            if (tracer == null) throw new ArgumentException($"{nameof(tracer)}=null");

            #endregion

            // Default
            _workService = workService;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            var message = _workService.GetValue();
            if (string.IsNullOrEmpty(message) == true) throw new InvalidOperationException($"{nameof(message)}=null");

            // ViewBag
            this.ViewBag.Message = message;

            // Return
            return View();
        }

        /// <summary>
        /// 測試API
        /// </summary>
        /// <param name="value">測試輸入</param>
        /// <returns>測試輸出</returns>
        public string Echo(string value)
        {
            #region Contracts

            if (string.IsNullOrEmpty(value) == true) throw new ArgumentException($"{nameof(value)}=null");

            #endregion

            // Return
            return value;
        }
    }
}
