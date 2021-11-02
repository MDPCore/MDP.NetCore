using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Lab
{
    public class HelloWorkService : WorkService
    {
        // Fields
        private readonly string _message = null;


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
        public void Execute()
        {
            // Display
            Console.WriteLine(_message);
        }

        public string GetValue()
        {
            // Return
            return _message;
        }
    }
}
