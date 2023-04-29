using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly MessageService _messageService;


        // Constructors
        public HomeController(MessageService messageService)
        {
            // Default
            _messageService = messageService;
        }


        // Methods
        public ActionResult<object> Index()
        {
            // Message
            var message = _messageService.GetValue();

            // Return
            return new { message= message };
        }
    }
}
