using MDP.Network.Rest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Factory<IServiceCollection, RestClientFactorySetting>("MDP.DevKit.OpenAI", "RestClientFactory")]
    public class RestClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, RestClientFactorySetting restClientFactorySetting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (restClientFactorySetting == null) throw new ArgumentException($"{nameof(restClientFactorySetting)}=null");

            #endregion

            // RestClientFactory
            serviceCollection.AddRestClientFactory("MDP.DevKit.OpenAI", restClientFactorySetting.Endpoints);
        }


        // Class
        public class RestClientFactorySetting
        {
            // Properties
            public Dictionary<string, RestClientEndpoint> Endpoints { get; set; } = null;
        }
    }
}
