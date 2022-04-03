using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting.Lab
{
    public class DecorateWorkService : WorkService
    {
        // Fields
        private readonly string _message = null;

        private readonly WorkService _workService = null;


        // Constructors
        public DecorateWorkService(string message, WorkService workService)
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException(nameof(message));
            if (workService == null) throw new ArgumentException(nameof(workService));

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
