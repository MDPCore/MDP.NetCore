using MDP.Network.Rest;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace MDP.Network.Rest
{
    [MDP.Registration.Factory<IServiceCollection, RestClientFactorySetting>("MDP.Network.Rest", "RestClientFactory")]
    public class RestClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, RestClientFactorySetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // RestClientFactory
            serviceCollection.AddRestClientFactory(setting.Endpoints);
        }


        // Class
        public class RestClientFactorySetting
        {
            // Properties
            public Dictionary<string, RestClientEndpoint>? Endpoints { get; set; } = null;
        }
    }
}
