using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace MDP.Network.Http
{
    [MDP.Registration.Factory<IServiceCollection, HttpClientFactorySetting>("MDP.Network.Http", "HttpClientFactory")]
    public class HttpClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, HttpClientFactorySetting httpClientFactorySetting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (httpClientFactorySetting == null) throw new ArgumentException($"{nameof(httpClientFactorySetting)}=null");

            #endregion

            // EndpointList
            var endpointList = httpClientFactorySetting.Endpoints ?? new Dictionary<string, HttpClientEndpoint>();
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
                endpoint.Value.Name = name;
            }

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(endpointList.Values.ToList());
        }


        // Class
        public class HttpClientFactorySetting
        {
            // Properties
            public Dictionary<string, HttpClientEndpoint>? Endpoints { get; set; } = null;
        }
    }
}
