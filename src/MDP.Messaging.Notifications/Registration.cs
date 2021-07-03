using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public class Registration
    {
        // Properties
        public string UserId { get; set; } = null;

        public string DeviceType { get; set; } = null;

        public string Token { get; set; } = null;
    }
}
