using Microsoft.AspNetCore.Mvc;

namespace MDP.AspNetCore.Lab
{
    [MDP.AspNetCore.Module]
    public class HomeController : Controller
    {
        // Fields
        private readonly WorkService _workService;


        // Constructors
        public HomeController(WorkService workService)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException($"{nameof(workService)}=null");

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
