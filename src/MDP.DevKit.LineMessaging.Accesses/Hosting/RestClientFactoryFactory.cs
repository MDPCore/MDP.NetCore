using MDP.Network.Rest;
using Microsoft.Extensions.DependencyInjection;

namespace MDP.DevKit.LineMessaging.Accesses
{
    [MDP.Registration.Factory<IServiceCollection, RestClientFactorySetting>("MDP.DevKit.LineMessaging", "RestClientFactory")]
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
            serviceCollection.AddRestClientFactory("MDP.DevKit.LineMessaging", setting.Endpoints);
        }


        // Class
        public class RestClientFactorySetting
        {
            // Properties
            public Dictionary<string, RestClientEndpoint>? Endpoints { get; set; } = null;
        }
    }
}
