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
using MDP.Hosting;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // NotificationContext
            containerBuilder.RegisterFactory<NotificationContext, NotificationContextFactory>(this.Configuration).SingleInstance();

            // RegistrationRepository
            containerBuilder.RegisterFactory<RegistrationRepository, MockRegistrationRepositoryFactory>(this.Configuration);
            containerBuilder.RegisterFactory<RegistrationRepository, SqlRegistrationRepositoryFactory>(this.Configuration);

            // NotificationProvider
            containerBuilder.RegisterFactory<NotificationProvider, MockNotificationProviderFactory>(this.Configuration);
            containerBuilder.RegisterFactory<NotificationProvider, FirebaseNotificationProviderFactory>(this.Configuration);
        }
    }
}
