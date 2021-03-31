using Autofac;
using MDP.Messaging.Notifications.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using MDP.Messaging.Notifications.Accesses;
using MDP.Messaging.Notifications.Firebase;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextModule : MDP.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // NotificationContext
            {
                // Register
                container.RegisterType<NotificationContext>().As<NotificationContext>()

                // Start
                .OnActivated((handler) =>
                {

                })

                // Lifetime
                .AutoActivate().SingleInstance();
            }

            // RegistrationRepository
            container.RegisterInterface<RegistrationRepository>();
            {
                // MockRegistrationRepository
                container.RegisterImplementation<RegistrationRepository, MockRegistrationRepository>();

                // SqlRegistrationRepository
                container.RegisterImplementation<RegistrationRepository, SqlRegistrationRepository>();
            }

            // NotificationProvider
            container.RegisterInterface<NotificationProvider>();
            {
                // MockNotificationProvider
                container.RegisterImplementation<NotificationProvider, MockNotificationProvider>();

                // FirebaseNotificationProvider
                container.RegisterImplementation<NotificationProvider, FirebaseNotificationProvider>();
            }
        }
    }
}
