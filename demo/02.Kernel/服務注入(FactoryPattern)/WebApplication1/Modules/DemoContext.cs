using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<DemoContext>()]
    public class DemoContext
    {
        // Fields
        private readonly DemoService _demoServiceByTyped;

        private readonly DemoService _demoServiceByNamed;

        private readonly DemoService _demoServiceByNamed001;

        private readonly DemoSetting _demoSetting;

        private readonly string _message;


        // Constructors
        public DemoContext
        (
            DemoService demoServiceByTyped,
            DemoService demoServiceByNamed,
            DemoService demoServiceByNamed001,
            DemoSetting demoSetting,
            string message
        )
        {
            // Default
            _demoServiceByTyped = demoServiceByTyped;
            _demoServiceByNamed = demoServiceByNamed;
            _demoServiceByNamed001 = demoServiceByNamed001;
            _demoSetting = demoSetting;
            _message = message;
        }


        // Methods
        public string GetMessage()
        {
            // Message
            var message = string.Empty;

            //  DemoService
            message += $"{_demoServiceByTyped.GetMessage()} by DemoServiceByTyped.GetMessage()" + Environment.NewLine;
            message += $"{_demoServiceByNamed.GetMessage()} by DemoServiceByNamed.GetMessage()" + Environment.NewLine;
            message += $"{_demoServiceByNamed001.GetMessage()} by DemoServiceByNamed[001].GetMessage()" + Environment.NewLine;
            message += Environment.NewLine;

            // DemoSetting
            message += $"{_demoSetting.Message} by DemoSetting.Message" + Environment.NewLine;
            message += Environment.NewLine;

            // DemoContext
            message += $"{_message} by DemoContext.Message" + Environment.NewLine;

            // Return
            return message;
        }
    }
}
