using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CLK.ComponentModel;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDP.Data.MSSql
{
    public class SqlClientFactoryFactory: ServiceFactory<IServiceCollection, SqlClientFactoryFactory.Setting>
    {
        // Constructors
        public SqlClientFactoryFactory() : base("MDP.Data.MSSql", "SqlClientFactory") { }

        protected SqlClientFactoryFactory(string @namespace, string service = null) : base(@namespace, service) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Require
            if (setting.Endpoints == null) setting.Endpoints = new Dictionary<string, SqlClientEndpoint>(StringComparer.OrdinalIgnoreCase);

            // EndpointList
            var endpointList = new List<SqlClientEndpoint>();
            {
                // Endpoint
                foreach (var endpointPair in setting.Endpoints)
                {
                    // Endpoint
                    var endpoint = endpointPair.Value;
                    if (endpoint == null) throw new InvalidOperationException($"{nameof(endpoint)}=null");

                    // Name
                    var name = endpointPair.Value.Name;
                    if (string.IsNullOrEmpty(name) == true)
                    {
                        name = endpointPair.Key;
                    }
                    endpoint.Name = name;
                    if (string.IsNullOrEmpty(endpoint.Name) == true) throw new InvalidOperationException($"{nameof(endpoint.Name)}=null");

                    // Add
                    endpointList.Add(endpoint);
                }
            }

            // AddSqlClientOptions
            foreach (var endpoint in endpointList)
            {
                serviceCollection.AddSingleton<SqlClientOptions>(serviceProvider =>
                {
                    // SqlClientOptions
                    var sqlClientOptions = new SqlClientOptions();
                    {
                        // Name
                        sqlClientOptions.Name = endpoint.Name;
                        if (string.IsNullOrEmpty(sqlClientOptions.Name) == true) throw new InvalidOperationException($"{nameof(sqlClientOptions.Name)}=null");

                        // ConnectionString
                        sqlClientOptions.ConnectionString = endpoint.ConnectionString;
                        if (string.IsNullOrEmpty(sqlClientOptions.ConnectionString) == true) throw new InvalidOperationException($"{nameof(sqlClientOptions.ConnectionString)}=null");

                        // SqlClientHandler                    
                        var handlerNameList = endpoint.Handlers ?? new List<string>();
                        foreach (var handlerName in handlerNameList)
                        {
                            // Resolve
                            var sqlClientHandler = serviceProvider.ResolveNamed<SqlClientHandler>(handlerName);
                            if (sqlClientHandler == null) throw new InvalidOperationException($"{nameof(sqlClientHandler)}=null");

                            // Add
                            sqlClientOptions.Handlers.Add(sqlClientHandler);
                        }
                    }

                    // Return
                    return sqlClientOptions;
                });
            }

            // AddSqlClientFactory
            serviceCollection.TryAddSingleton<SqlClientFactory, SqlClientFactory>();
        }


        // Class
        public class Setting
        {
            // Properties
            public Dictionary<string, SqlClientEndpoint> Endpoints { get; set; } = null;
        }
    }
}
