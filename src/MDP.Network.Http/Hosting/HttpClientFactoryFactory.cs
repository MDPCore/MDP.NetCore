using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MDP.Network.Http
{
    public class HttpClientFactoryFactory: Factory<IServiceCollection, HttpClientFactoryFactory.Setting>
    {
        // Constructors
        public HttpClientFactoryFactory() : base("MDP.Network.Http", "HttpClientFactory") { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(setting.Endpoints);
        }


        // Class
        public class Setting
        {
            // Properties
            public Dictionary<string, HttpClientEndpoint> Endpoints { get; set; } = null;
        }
    }
}
