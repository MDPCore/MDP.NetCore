﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Firebase
{
    public class FirebaseNotificationProvider : NotificationProvider
    {
        // Constructors
        public FirebaseNotificationProvider() 
        {
            // Default

        }


        // Methods
        public void Send(Notification notification, List<Registration> registrationList)
        {
            #region Contracts

            if (notification == null) throw new ArgumentException(nameof(notification));
            if (registrationList == null) throw new ArgumentException(nameof(registrationList));

            #endregion

        }
    }
}
