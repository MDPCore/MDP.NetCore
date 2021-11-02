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
        protected override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // NotificationContext
            containerBuilder.RegisterService<NotificationContext>().SingleInstance();
            containerBuilder.RegisterFactory<NotificationContext, NotificationContextFactory>();

            // RegistrationRepository
            containerBuilder.RegisterService<RegistrationRepository>();
            containerBuilder.RegisterFactory<RegistrationRepository, MockRegistrationRepositoryFactory>();
            containerBuilder.RegisterFactory<RegistrationRepository, SqlRegistrationRepositoryFactory>();

            // NotificationProvider
            containerBuilder.RegisterService<NotificationProvider>();
            containerBuilder.RegisterFactory<NotificationProvider, MockNotificationProviderFactory>();
            containerBuilder.RegisterFactory<NotificationProvider, FirebaseNotificationProviderFactory>();
        }
    }
}
