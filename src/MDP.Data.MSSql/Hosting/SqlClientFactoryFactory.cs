using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDP.Data.MSSql
{
    public class SqlClientFactoryFactory: ServiceFactory<IServiceCollection, SqlClientFactoryFactory.Setting>
    {
        // Constructors
        public SqlClientFactoryFactory() : base("MDP.Data.MSSql", "SqlClientFactory", false) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // SqlClientFactory
            serviceCollection.TryAddSingleton<SqlClientFactory, SqlClientFactory>();

            // SqlClient
            foreach (var endpoint in setting)
            {
                // Require
                if (string.IsNullOrEmpty(endpoint.Key) == true) throw new InvalidOperationException($"{nameof(endpoint.Key)}=null");
                if (endpoint.Value == null) throw new InvalidOperationException($"{nameof(endpoint.Value)}=null");
                if (endpoint.Value.Handlers == null) endpoint.Value.Handlers = new List<string>();

                // SqlClientBuilder
                serviceCollection.AddSingleton<SqlClientBuilder>(serviceProvider =>
                {
                    // SqlClientBuilder
                    var sqlClientBuilder = new SqlClientBuilder
                    (
                        endpoint.Key,
                        endpoint.Value.ConnectionString
                    );

                    // SqlClientHandler                    
                    foreach (var handler in endpoint.Value.Handlers)
                    {
                        // Resolve
                        var sqlClientHandler = serviceProvider.ResolveNamed<SqlClientHandler>(handler);
                        if (sqlClientHandler == null) throw new InvalidOperationException($"{nameof(sqlClientHandler)}=null");

                        // Add
                        sqlClientBuilder.Handlers.Add(sqlClientHandler);
                    }

                    // Return
                    return sqlClientBuilder;
                });
            }
        }


        // Class
        public class Setting: Dictionary<string, Endpoint>
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
