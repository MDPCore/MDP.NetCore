using Microsoft.Extensions.DependencyInjection;
using MDP.Network.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

namespace MDP.Network.Rest
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static void AddRestClientFactory(this IServiceCollection serviceCollection, Dictionary<string, RestClientEndpoint>? endpointDictionary = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // AddRestClientFactory
            serviceCollection.AddRestClientFactory(null, endpointDictionary);
        }

        public static void AddRestClientFactory(this IServiceCollection serviceCollection, string? @namespace = null, Dictionary<string, RestClientEndpoint>? endpointDictionary = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // RestClientFactory
            serviceCollection.TryAddSingleton<RestClientFactory>();

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(@namespace, endpointDictionary?.ToDictionary
            (
                endpointPair => endpointPair.Key,
                endpointPair => (HttpClientEndpoint)endpointPair.Value)
            );
        }


        public static void AddRestClientFactory(this IServiceCollection serviceCollection, List<RestClientEndpoint>? endpointList = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // AddRestClientFactory
            serviceCollection.AddRestClientFactory(null, endpointList);
        }

        public static void AddRestClientFactory(this IServiceCollection serviceCollection, string? @namespace = null, List<RestClientEndpoint>? endpointList = null)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // RestClientFactory
            serviceCollection.TryAddSingleton<RestClientFactory>();

            // RestClientFactory
            serviceCollection.AddHttpClientFactory(@namespace, endpointList?.OfType<HttpClientEndpoint>().ToList());
        }
    }
}
