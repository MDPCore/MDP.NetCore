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
using CLK.Autofac;
using MDP.Hosting;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextModule : MDP.Hosting.Module
    {
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public NotificationContextModule(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Default
            _configuration = configuration;
        }


        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // NotificationContext
            container.RegisterServiceType<NotificationContext, NotificationContext, NotificationContextFactory, NotificationContextOptions>(_configuration, (builder) => builder.SingleInstance());

            // Firebase
            container.RegisterServiceType<NotificationProvider, FirebaseNotificationProvider, FirebaseNotificationProviderFactory>();

            // Accesses
            container.RegisterServiceType<RegistrationRepository, SqlRegistrationRepository, SqlRegistrationRepositoryFactory>();
            
            // Mocks
            container.RegisterServiceType<RegistrationRepository, MockRegistrationRepository, MockRegistrationRepositoryFactory, MockRegistrationRepositoryOptions>(_configuration);
            container.RegisterServiceType<NotificationProvider, MockNotificationProvider, MockNotificationProviderFactory, MockNotificationProviderOptions>(_configuration);
        }
    }
}
