using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public interface NotificationProvider
    {
        // Methods
        void Send(Notification notification, List<Registration> registrationList);
    }
}
