using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Firebase
{
    public abstract class FirebaseNotificationFormatter
    {
        // Constructors
        public FirebaseNotificationFormatter(string notificationType, string deviceType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(notificationType) == true) throw new ArgumentException(nameof(notificationType));
            if (string.IsNullOrEmpty(deviceType) == true) throw new ArgumentException(nameof(deviceType));

            #endregion

            // Default
            this.NotificationType = notificationType;
            this.DeviceType = deviceType;
        }


        // Properties
        public string NotificationType { get; private set; }

        public string DeviceType { get; private set; }


        // Methods
        public abstract void Configure(FirebaseAdmin.Messaging.MulticastMessage message);
    }
}
