using MDP.Registration;

namespace MyLab.Modules
{
    [Register<WorkService>()]
    public class DecorateWorkService : WorkService
    {
        // Fields
        private readonly string _message;

        private readonly WorkService _workService;


        // Constructors
        public DecorateWorkService(string message, WorkService workService)
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException($"{nameof(message)}=null");
            if (workService == null) throw new ArgumentException($"{nameof(workService)}=null");

            #endregion

            // Default
            _message = message;
            _workService = workService;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _workService.GetValue() + " " + _message;
        }
    }
}
