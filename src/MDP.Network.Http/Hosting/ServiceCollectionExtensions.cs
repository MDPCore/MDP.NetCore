using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.Network.Http
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static void AddHttpClientFactory(this IServiceCollection serviceCollection)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // AddHttpClientFactory
            serviceCollection.AddHttpClientFactory(new List<HttpClientEndpoint>());
        }

        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, Dictionary<string, HttpClientEndpoint> endpointDictionary)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // EndpointList
            var endpointList = new List<HttpClientEndpoint>();
            {
                // Endpoint
                foreach (var endpointPair in endpointDictionary)
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

            // AddHttpClientFactory
            serviceCollection.AddHttpClientFactory(endpointList);
        }

        public static void AddHttpClientFactory(this IServiceCollection serviceCollection, List<HttpClientEndpoint> endpointList)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // Default
            serviceCollection.AddHttpClient();

            // EndpointList
            foreach (var endpoint in endpointList)
            {
                // Name
                var name = endpoint.Name;
                if (string.IsNullOrEmpty(name) == true) throw new InvalidOperationException($"{nameof(name)}=null");

                // HttpClient
                var httpClientBuilder = serviceCollection.AddHttpClient(name, httpClient =>
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

                // HttpMessageHandler
                //{
                //    // Handlers
                //    var handlers = endpoint.Handlers ?? new List<string>();
                //    foreach (var handler in handlers)
                //    {
                //        // Add
                //        httpClientBuilder.AddHttpMessageHandler(serviceProvider => {
                //            return null;
                //        });
                //    }
                //}
            }
        }
    }
}
