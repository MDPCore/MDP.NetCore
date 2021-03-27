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
            {
                // RegistrationRepository
                container.RegisterNamed<RegistrationRepository>(componentContext =>
                {
                    return "Mock";
                });

                // SqlRegistrationRepository
                container.RegisterType<SqlRegistrationRepository>().Named<RegistrationRepository>("Sql");

                // MockRegistrationRepository
                container.RegisterType<MockRegistrationRepository>().Named<RegistrationRepository>("Mock");
            }

            // NotificationProvider
            {
                // NotificationProvider
                container.RegisterNamed<NotificationProvider>(componentContext =>
                {
                    return "Firebase";
                });

                // SqlNotificationProvider
                container.RegisterType<FirebaseNotificationProvider>().Named<NotificationProvider>("Firebase");

                // MockNotificationProvider
                container.RegisterType<MockNotificationProvider>().Named<NotificationProvider>("Mock");
            }
        }
    }
}
