using Microsoft.Extensions.DependencyInjection;
using static MDP.Network.Http.HttpClientFactoryFactory;

namespace MDP.Network.Http
{
    public static class HttpClientFactoryExtensions
    {
        // Methods
        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, List<HttpClientEndpoint>? endpointList = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // EndpointList
            endpointList = endpointList ?? new List<HttpClientEndpoint>();
            if (endpointList.Count <= 0)
            {
                // HttpClient
                serviceCollection.AddHttpClient();

                // Return
                return;
            }

            // HttpClientFactory
            foreach (var endpoint in endpointList)
            {
                // Name
                var name = endpoint.Name;
                if (string.IsNullOrEmpty(name) == true) throw new InvalidOperationException($"{nameof(name)}=null");

                // HttpClient
                serviceCollection.AddHttpClient(name, httpClient =>
                {
                    // BaseAddress
                    var baseAddress = endpoint.BaseAddress;
                    if (string.IsNullOrEmpty(baseAddress) == false)
                    {
                        // EndsWith
                        if (baseAddress.EndsWith(@"/") == false) baseAddress += @"/";

                        // Set
                        httpClient.BaseAddress = new Uri(baseAddress);
                    }

                    // Headers
                    var headers = endpoint.Headers ?? new Dictionary<string, string>();
                    foreach (var header in headers)
                    {
                        // Add
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                });
            }
        }
    }
}
