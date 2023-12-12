using MDP.Network.Rest;
using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static MDP.Network.Rest.RestClientFactoryFactory;

namespace MDP.Network.Rest
{
    public class RestClientFactoryFactory : Factory<IServiceCollection, RestClientFactoryFactory.Setting>
    {
        // Constructors
        public RestClientFactoryFactory() : base("MDP.Network.Rest", "RestClientFactory") { }


        // Methods
        public override List<ServiceRegistration> ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // RestClientFactory
            serviceCollection.AddRestClientFactory(setting.Endpoints);

            // Return
            return null;
        }


        // Class
        public class Setting
        {
            // Properties
            public Dictionary<string, RestClientEndpoint> Endpoints { get; set; } = null;
        }
    }
}
