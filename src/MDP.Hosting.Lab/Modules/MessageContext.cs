using MDP.Registration;

namespace MyLab.Module
{
    [Service<MessageContext>(singleton: true)]
    public class MessageContext
    {
        // Fields
        private readonly MessageRepository _messageRepository = null;


        // Constructors
        public MessageContext(MessageRepository messageRepository)
        {
            // Default
            _messageRepository = messageRepository;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _messageRepository.GetValue();
        }
    }
}
