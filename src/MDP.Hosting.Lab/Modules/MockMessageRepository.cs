using MDP.Registration;

namespace MyLab.Module
{
    [Service<MessageRepository>()]
    public class MockMessageRepository : MessageRepository
    {
        // Methods
        public string GetValue()
        {
            // Return
            return "Hello World By Mock";
        }
    }
}
