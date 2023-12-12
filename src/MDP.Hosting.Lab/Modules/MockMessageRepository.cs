namespace MyLab.Module
{
    [MDP.Registration.Service<MessageRepository>()]
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
