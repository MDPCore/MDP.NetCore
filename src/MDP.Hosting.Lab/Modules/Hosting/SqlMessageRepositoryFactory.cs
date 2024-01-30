using MDP.Hosting;
using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MyLab.Module
{
    public class SqlMessageRepositoryFactory : ServiceFactory<ServiceBuilder, SqlMessageRepositoryFactory.Setting>
    {
        // Constructors
        public SqlMessageRepositoryFactory() : base("MyLab.Module", "SqlMessageRepository") { }


        // Methods
        public override void ConfigureService(ServiceBuilder serviceBuilder, Setting setting)
        {
            #region Contracts

            if (serviceBuilder == null) throw new ArgumentException($"{nameof(serviceBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // SqlMessageRepository
            serviceBuilder.Add(new ServiceRegistration()
            {
                ServiceType = typeof(MessageRepository),
                InstanceType = typeof(SqlMessageRepository),
                InstanceName = nameof(SqlMessageRepository),
                Parameters = new Dictionary<string, object>
                {
                    { "Message" , setting.Message}
                },
                Singleton = false,
            });
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; }
        }
    }
}