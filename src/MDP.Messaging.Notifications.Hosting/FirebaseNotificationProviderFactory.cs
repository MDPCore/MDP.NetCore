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
    public class FirebaseNotificationProviderFactory : Factory<NotificationProvider, FirebaseNotificationProvider>
    {
        // Methods
        protected override FirebaseNotificationProvider CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Create
            return new FirebaseNotificationProvider
            (
                componentContext.Resolve<FirebaseMessaging>()
            );
        }
    }
}
