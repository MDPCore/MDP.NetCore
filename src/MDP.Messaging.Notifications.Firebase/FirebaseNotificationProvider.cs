using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Firebase
{
    public class FirebaseNotificationProvider : NotificationProvider
    {
        // Fields
        private readonly FirebaseMessaging _firebaseMessaging = null;

        private readonly List<FirebaseNotificationFormatter> _firebaseFormatterList = null;


        // Constructors
        public FirebaseNotificationProvider(FirebaseMessaging firebaseMessaging, List<FirebaseNotificationFormatter> firebaseFormatterList = null)
        {
            #region Contracts

            if (firebaseMessaging == null) throw new ArgumentException(nameof(firebaseMessaging));

            #endregion

            // Default
            _firebaseMessaging = firebaseMessaging;
            _firebaseFormatterList = firebaseFormatterList ?? new List<FirebaseNotificationFormatter>();
        }


        // Methods
        public void Send(Notification notification, List<Registration> registrationList)
        {
            #region Contracts

            if (notification == null) throw new ArgumentException(nameof(notification));
            if (registrationList == null) throw new ArgumentException(nameof(registrationList));

            #endregion

            // Message
            var message = new FirebaseAdmin.Messaging.MulticastMessage()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = notification.Title,
                    Body = notification.Text,
                },
                Tokens = registrationList.Select(registration=> registration.Token).ToList()
            };

            // Formatter
            foreach(var firebaseFormatter in _firebaseFormatterList.Where(x=>x.NotificationType== notification.Type))
            {
                // Configure
                firebaseFormatter.Configure(message);
            }

            // Send
            var response = _firebaseMessaging.SendMulticastAsync(message).GetAwaiter().GetResult();
            if (response == null) throw new InvalidOperationException($"{nameof(response)}=null");
        }
    }
}
