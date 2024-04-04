using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Net.Http;

namespace MDP.Network.Http
{
    public class HttpClientFactoryFactory: ServiceFactory<IServiceCollection, HttpClientFactoryFactory.Setting>
    {
        // Constructors
        public HttpClientFactoryFactory() : base("MDP.Network.Http", "HttpClientFactory", false) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Setting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // HttpClientFactory
            serviceCollection.AddHttpClient();

            // HttpClient
            foreach (var endpoint in setting)
            {
                // Require
                if (string.IsNullOrEmpty(endpoint.Key) == true) throw new InvalidOperationException($"{nameof(endpoint.Key)}=null");
                if (endpoint.Value == null) throw new InvalidOperationException($"{nameof(endpoint.Value)}=null");
                if (endpoint.Value.Handlers == null) endpoint.Value.Handlers = new List<string>();
                if (endpoint.Value.Headers == null) endpoint.Value.Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // HttpClientBuilder
                var httpClientBuilder = serviceCollection.AddHttpClient(endpoint.Key, httpClient =>
                {
                    // BaseAddress
                    var baseAddress = endpoint.Value.BaseAddress;
                    if (string.IsNullOrEmpty(baseAddress) == false)
                    {
                        // EndsWith
                        if (baseAddress.EndsWith(@"/") == false) baseAddress += @"/";

                        // Set
                        httpClient.BaseAddress = new Uri(baseAddress);
                    }

                    // Headers
                    foreach (var header in endpoint.Value.Headers)
                    {
                        // Add
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                });

                // HttpClientHandler
                foreach (var handler in endpoint.Value.Handlers)
                {
                    // Add
                    httpClientBuilder = httpClientBuilder.AddHttpMessageHandler(serviceProvider =>
                    {
                        // Resolve
                        var httpClientHandler = serviceProvider.ResolveNamed<HttpClientHandler>(handler);
                        if (httpClientHandler == null) throw new InvalidOperationException($"{nameof(httpClientHandler)}=null");

                        // Return
                        return httpClientHandler;
                    });
                }
            }
        }


        // Class
        public class Setting : Dictionary<string, Endpoint>
        {

        }

        public class Endpoint
        {
            // Properties
            public string BaseAddress { get; set; } = string.Empty;

            public List<string> Handlers { get; set; } = new List<string>();

            public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
