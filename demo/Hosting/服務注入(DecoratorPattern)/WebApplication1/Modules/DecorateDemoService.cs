using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class DecorateDemoService : DemoService
    {
        // Fields
        private readonly string _message;

        private readonly DemoService _component ;


        // Constructors
        public DecorateDemoService(string message, DemoService component)
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException($"{nameof(message)}=null");
            if (component  == null) throw new ArgumentException($"{nameof(component )}=null");

            #endregion

            // Default
            _message = message;
            _component  = component ;
        }


        // Methods
        public string GetMessage()
        {
            // Return
            return _message + "-->" + _component.GetMessage();
        }
    }
}
