using Microsoft.Extensions.DependencyInjection;

namespace MDP.Network.Http
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, Dictionary<string, HttpClientEndpoint>? endpointDictionary = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // AddHttpClientFactory
            serviceCollection.AddHttpClientFactory(null, endpointDictionary);
        }

        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, string? @namespace = null, Dictionary<string, HttpClientEndpoint>? endpointDictionary = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // EndpointList
            var endpointList = new List<HttpClientEndpoint>();

            // EndpointDictionary
            endpointDictionary = endpointDictionary ?? new Dictionary<string, HttpClientEndpoint>();
            foreach (var endpointPair in endpointDictionary)
            {
                // Require
                if (endpointPair.Value == null) throw new InvalidOperationException($"{nameof(endpointPair.Value)}=null");
                if (string.IsNullOrEmpty(endpointPair.Key) == true) throw new InvalidOperationException($"{nameof(endpointPair.Key)}=null");

                // Endpoint
                var endpoint = endpointPair.Value.Clone();
                if (endpoint == null) throw new InvalidOperationException($"{nameof(endpoint)}=null");

                // Name
                var name = endpointPair.Value.Name;
                if (string.IsNullOrEmpty(name) == true)
                {
                    name = endpointPair.Key;
                }
                endpoint.Name = name;

                // Add
                endpointList.Add(endpoint);
            }

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(@namespace, endpointList);
        }


        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, List<HttpClientEndpoint>? endpointList = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // AddHttpClientFactory
            serviceCollection.AddHttpClientFactory(null, endpointList);
        }

        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, string? @namespace = null, List<HttpClientEndpoint>? endpointList = null)
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
                string? name = null;
                if (string.IsNullOrEmpty(@namespace) == true) name = $"{endpoint.Name}";
                if (string.IsNullOrEmpty(@namespace) == false) name = $"{@namespace}.{endpoint.Name}";
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
