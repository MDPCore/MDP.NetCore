using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class FixedDemoService : DemoService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public FixedDemoService(string message)
        {
            // Default
            _message = message;
        }


        // Methods
        public string GetMessage()
        {
            // Return
            return _message;
        }
    }
}
