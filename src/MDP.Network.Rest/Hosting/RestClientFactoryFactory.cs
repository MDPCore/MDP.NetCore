using MDP.Network.Http;
using MDP.Network.Rest;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using static MDP.Network.Http.HttpClientFactoryFactory;

namespace MDP.Network.Rest
{
    [MDP.Registration.Factory<IServiceCollection, RestClientFactorySetting>("MDP.Network.Rest", "RestClientFactory")]
    public class RestClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, RestClientFactorySetting restClientFactorySetting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (restClientFactorySetting == null) throw new ArgumentException($"{nameof(restClientFactorySetting)}=null");

            #endregion

            // EndpointList
            var endpointList = restClientFactorySetting.Endpoints ?? new Dictionary<string, HttpClientEndpoint>();
            foreach (var endpoint in endpointList)
            {
                // Require
                if (endpoint.Value == null) throw new InvalidOperationException($"{nameof(endpoint.Value)}=null");
                if (string.IsNullOrEmpty(endpoint.Key) == true) throw new InvalidOperationException($"{nameof(endpoint.Key)}=null");

                // Name
                var name = endpoint.Value.Name;
                if (string.IsNullOrEmpty(name) == true)
                {
                    name = endpoint.Key;
                }
                endpoint.Value.Name = RestClientDefaults.CreateName(name);
            }

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(endpointList.Values.ToList());
        }


        // Class
        public class RestClientFactorySetting
        {
            // Properties
            public Dictionary<string, HttpClientEndpoint>? Endpoints { get; set; } = null;
        }
    }
}
