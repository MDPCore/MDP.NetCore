using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace MDP.Network.Http
{
    [MDP.Registration.Factory<IServiceCollection, HttpClientFactorySetting>("MDP.Network.Http", "HttpClientFactory")]
    public class HttpClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, HttpClientFactorySetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // HttpClientFactory
            serviceCollection.AddHttpClientFactory(setting.Endpoints);
        }


        // Class
        public class HttpClientFactorySetting
        {
            // Properties
            public Dictionary<string, HttpClientEndpoint>? Endpoints { get; set; } = null;
        }
    }
}
