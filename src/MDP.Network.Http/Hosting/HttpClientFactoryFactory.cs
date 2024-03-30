using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CLK.ComponentModel;
using System.Net.Http;

namespace MDP.Network.Http
{
    public class HttpClientFactoryFactory: ServiceFactory<IServiceCollection, HttpClientFactoryFactory.Setting>
    {
        // Constructors
        public HttpClientFactoryFactory() : base("MDP.Network.Http", "HttpClientFactory") { }

        protected HttpClientFactoryFactory(string @namespace, string service = null) : base(@namespace, service) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Require
            if (setting.Endpoints == null) setting.Endpoints = new Dictionary<string, HttpClientEndpoint>(StringComparer.OrdinalIgnoreCase);

            // EndpointList
            var endpointList = new List<HttpClientEndpoint>();
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

            // AddHttpClient
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
                    var headers = endpoint.Headers ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var header in headers)
                    {
                        // Add
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                });

                // HttpMessageHandler
                var handlerNameList = endpoint.Handlers ?? new List<string>();
                foreach (var handlerName in handlerNameList)
                {
                    // Add
                    httpClientBuilder = httpClientBuilder.AddHttpMessageHandler(serviceProvider =>
                    {
                        // Resolve
                        var httpMessageHandler = serviceProvider.ResolveNamed<DelegatingHandler>(handlerName);
                        if (httpMessageHandler == null) throw new InvalidOperationException($"{nameof(httpMessageHandler)}=null");

                        // Return
                        return httpMessageHandler;
                    });
                }
            }

            // Default
            serviceCollection.AddHttpClient();
        }


        // Class
        public class Setting
        {
            // Properties
            public Dictionary<string, HttpClientEndpoint> Endpoints { get; set; } = null;
        }
    }
}
