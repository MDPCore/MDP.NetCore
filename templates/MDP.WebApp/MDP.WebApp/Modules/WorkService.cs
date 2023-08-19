using MDP.Registration;

namespace MDP.WebApp
{
    public interface WorkService
    {
        // Methods
        string GetValue();
    }

    [Service<WorkService>()]
    public class MessageService : WorkService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public MessageService(string message)
        {
            // Default
            _message = message;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _message;
        }
    }
}
