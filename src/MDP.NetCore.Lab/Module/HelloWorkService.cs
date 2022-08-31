namespace MDP.NetCore.Lab
{
    public class HelloWorkService : WorkService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public HelloWorkService(string message)
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException(nameof(message));

            #endregion

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
