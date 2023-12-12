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
        public override List<ServiceRegistration> ConfigureService(IServiceCollection builder, Setting setting)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException($"{nameof(builder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // SqlMessageRepository
            var serviceRegistration = new ServiceRegistration()
            {
                ServiceType = typeof(MessageRepository),
                InstanceType = typeof(SqlMessageRepository),
                InstanceName = nameof(SqlMessageRepository),
                Parameters = new Dictionary<string, object>
                {
                    { "Message" , setting.Message}
                },
                Singleton = false,
            };

            // Return
            return new List<ServiceRegistration>() { serviceRegistration };
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; }
        }
    }
}