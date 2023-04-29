using MDP.Registration;

namespace WebApplication1
{
    [Service<MessageService>()]
    public class MessageService
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
