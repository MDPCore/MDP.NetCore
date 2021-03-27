using CLK;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public class NotificationContext
    {
        // Fields
        private readonly RegistrationRepository _registrationRepository = null;

        private readonly NotificationProvider _notificationProvider = null;


        // Constructors
        public NotificationContext(RegistrationRepository registrationRepository, NotificationProvider notificationProvider)
        {
            #region Contracts

            if (registrationRepository == null) throw new ArgumentException(nameof(registrationRepository));
            if (notificationProvider == null) throw new ArgumentException(nameof(notificationProvider));

            #endregion

            // Default
            _registrationRepository = registrationRepository;
            _notificationProvider = notificationProvider;
        }
        

        // Methods
        public void Register(Registration registration)
        {
            #region Contracts

            if (registration == null || registration.IsValid() == false) throw new ArgumentException(nameof(registration));

            #endregion

            // Register
            try
            {
                // Add
                _registrationRepository.Add(registration);

                // Return
                return;
            }
            catch (DuplicateKeyException) 
            {
                // Update
                _registrationRepository.Update(registration);

                // Return
                return;
            }
        }

        public void Unregister(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Remove
            _registrationRepository.RemoveAllByUserId(userId);
        }

        public void Send(Notification notification, List<string> userIdList)
        {
            #region Contracts

            if (notification == null || notification.IsValid() == false) throw new ArgumentException(nameof(notification));
            if (userIdList == null) throw new ArgumentException(nameof(notification));

            #endregion

            // Require
            if (userIdList.Count <= 0) return;

            // Registration
            var registrationList = _registrationRepository.FindAllByUserId(userIdList);
            if (registrationList == null) throw new InvalidOperationException($"{nameof(registrationList)}=null");
            if (registrationList.Count == 0) return;

            // Send
            _notificationProvider.Send(notification, registrationList);
        }
    }
}
