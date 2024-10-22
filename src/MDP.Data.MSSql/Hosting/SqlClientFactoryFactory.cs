using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDP.Data.MSSql
{
    public class SqlClientFactoryFactory: ServiceFactory<IServiceCollection, SqlClientFactoryFactory.SettingDictionary>
    {
        // Constructors
        public SqlClientFactoryFactory() : base("MDP.Data.MSSql", "SqlClientFactory", false) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, SettingDictionary settingDictionary)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (settingDictionary == null) throw new ArgumentException($"{nameof(settingDictionary)}=null");

            #endregion

            // SqlClientFactory
            serviceCollection.TryAddSingleton<SqlClientFactory, SqlClientFactory>();

            // SqlClientBuilder
            foreach (var setting in settingDictionary)
            {
                // Require
                if (string.IsNullOrEmpty(setting.Key) == true) throw new InvalidOperationException($"{nameof(setting.Key)}=null");
                if (setting.Value == null) throw new InvalidOperationException($"{nameof(setting.Value)}=null");
                if (setting.Value.Handlers == null) setting.Value.Handlers = new List<string>();
                if (string.IsNullOrEmpty(setting.Value.ConnectionString) == true) throw new ArgumentException($"{nameof(setting.Value.ConnectionString)}=null");
                                
                // Add
                serviceCollection.AddSingleton<SqlClientBuilder>(serviceProvider =>
                {
                    // SqlClientBuilder
                    var sqlClientBuilder = new SqlClientBuilder
                    (
                        setting.Key,
                        setting.Value.ConnectionString
                    );

                    // SqlClientHandler 
                    if (setting.Value.Handlers.Count <= 0)
                    {
                        // Typed
                        var sqlClientHandlerList = serviceProvider.GetServices<SqlClientHandler>();
                        if (sqlClientHandlerList == null) throw new InvalidOperationException($"{nameof(sqlClientHandlerList)}=null");

                        // foreach
                        foreach (var sqlClientHandler in sqlClientHandlerList)
                        {
                            // Add
                            sqlClientBuilder.Handlers.Add(sqlClientHandler);
                        }
                    }
                    else
                    {
                        // Named                   
                        foreach (var handlerName in setting.Value.Handlers)
                        {
                            // Resolve
                            var sqlClientHandler = serviceProvider.ResolveNamed<SqlClientHandler>(handlerName);
                            if (sqlClientHandler == null) throw new InvalidOperationException($"{nameof(sqlClientHandler)}=null");

                            // Add
                            sqlClientBuilder.Handlers.Add(sqlClientHandler);
                        }
                    }

                    // Return
                    return sqlClientBuilder;
                });
            }
        }


        // Class
        public class SettingDictionary : Dictionary<string, Endpoint>
        {
            
        }

        public class Endpoint
        {
            // Properties
            public string ConnectionString { get; set; } = string.Empty;

            public List<string> Handlers { get; set; } = new List<string>();
        }
    }
}
