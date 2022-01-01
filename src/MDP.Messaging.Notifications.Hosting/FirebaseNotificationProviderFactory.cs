using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Firebase;
using FirebaseAdmin.Messaging;

namespace MDP.Messaging.Notifications.Hosting
{
    public class FirebaseNotificationProviderFactory : Factory<NotificationProvider, FirebaseNotificationProvider, FirebaseNotificationProviderFactory.Setting>
    {
        // Methods
        protected override FirebaseNotificationProvider CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new FirebaseNotificationProvider
            (
                componentContext.Resolve<FirebaseMessaging>()
            );
        }


        // Class
        public class Setting
        {

        }
    }
}
