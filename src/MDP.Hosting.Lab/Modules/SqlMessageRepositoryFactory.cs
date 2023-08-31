using MDP.Hosting;
using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyLab.Module
{
    [Factory<IServiceCollection, Setting>("MyLab.Module", "SqlMessageRepository")]
    public class SqlMessageRepositoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection builder, Setting setting)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException($"{nameof(builder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // RegisterTyped
            builder.RegisterTyped<MessageRepository>((serviceProvider) =>
            {
                return serviceProvider.ResolveNamed<MessageRepository>("SqlMessageRepository");
            });

            // RegisterNamed
            builder.RegisterNamed<MessageRepository>("SqlMessageRepository", (serviceProvider) =>
            {
                return new SqlMessageRepository
                (
                    message: setting.Message
                );
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