using MDP.DevKit.LineMessaging.Accesses;
using MDP.Network.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDP.DevKit.LineMessaging.Hosting
{
    [MDP.Registration.Factory<IServiceCollection, LineMessageContextSetting>("MDP.DevKit.LineMessaging", "LineMessageContextFactory")]
    public class LineMessageContextFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, LineMessageContextSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Domain
            serviceCollection.TryAddSingleton<LineMessageContext>();

            // Accesses
            serviceCollection.TryAddTransient<EventService, RestEventService>();
            serviceCollection.TryAddTransient<SignatureService>(serviceProvider => { return new RestSignatureService(setting.ChannelSecret); });
            serviceCollection.TryAddTransient<UserService, RestUserService>();
            serviceCollection.TryAddTransient<ContentService, RestContentService>();
            serviceCollection.TryAddTransient<MessageService, RestMessageService>();

            // RestClientFactory
            serviceCollection.AddRestClientFactory("MDP.DevKit.LineMessaging", new List<RestClientEndpoint>()
            {
                new RestClientEndpoint(){ Name= "LineMessageService", BaseAddress=@"https://api.line.me/v2/bot/", Headers = new Dictionary<string, string>(){ {"Authorization", $"Bearer {setting.ChannelAccessToken}" } } },
                new RestClientEndpoint(){ Name= "LineContentService", BaseAddress=@"https://api-data.line.me/v2/bot/", Headers = new Dictionary<string, string>(){ {"Authorization", $"Bearer {setting.ChannelAccessToken}" } } }
            });
        }


        // Class
        public class LineMessageContextSetting
        {
            // Properties
            public string ChannelSecret { get; set; }

            public string ChannelAccessToken { get; set; }
        }
    }
}
