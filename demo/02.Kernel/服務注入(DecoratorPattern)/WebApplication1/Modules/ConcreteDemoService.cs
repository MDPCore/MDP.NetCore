using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class ConcreteDemoService : DemoService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public ConcreteDemoService(string message)
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
