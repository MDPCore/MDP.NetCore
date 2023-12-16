using MDP.Hosting;
using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MyLab.Module
{
    public class SqlMessageRepositoryFactory : Factory<IServiceCollection, SqlMessageRepositoryFactory.Setting>
    {
        // Constructors
        public SqlMessageRepositoryFactory() : base("MyLab.Module", "SqlMessageRepository") { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // SqlMessageRepository
            serviceCollection.AddSingleton(new ServiceRegistration()
            {
                BuilderType = typeof(IServiceCollection),
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