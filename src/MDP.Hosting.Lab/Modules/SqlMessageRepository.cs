namespace MyLab.Module
{
    public class SqlMessageRepository : MessageRepository
    {
        // Fields
        private readonly string _message;


        // Constructors
        public SqlMessageRepository(string message)
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
