using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace MDP.Security.Tokens.Jwt
{
    public class TokenProviderFactoryFactory : ServiceFactory<IServiceCollection, TokenProviderFactoryFactory.SettingDictionary>
    {
        // Constructors
        public TokenProviderFactoryFactory() : base("MDP.Security.Tokens.Jwt", "TokenProviderFactory", false) { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, SettingDictionary settingDictionary)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (settingDictionary == null) throw new ArgumentException($"{nameof(settingDictionary)}=null");

            #endregion

            // TokenProviderFactory
            serviceCollection.TryAddSingleton<TokenProviderFactory, TokenProviderFactory>();

            // TokenProviderBuilder
            foreach (var setting in settingDictionary)
            {
                // Require
                if (string.IsNullOrEmpty(setting.Key) == true) throw new InvalidOperationException($"{nameof(setting.Key)}=null");
                if (setting.Value == null) throw new InvalidOperationException($"{nameof(setting.Value)}=null");
                if (string.IsNullOrEmpty(setting.Value.Algorithm) == true) throw new ArgumentException($"{nameof(setting.Value.Algorithm)}=null");
                if (string.IsNullOrEmpty(setting.Value.SignKey) == true) throw new ArgumentException($"{nameof(setting.Value.SignKey)}=null");

                // Add
                serviceCollection.AddSingleton<TokenProviderBuilder>(serviceProvider =>
                {
                    // TokenProviderBuilder
                    var sqlClientBuilder = new TokenProviderBuilder
                    (
                        setting.Key,
                        setting.Value.Algorithm,
                        setting.Value.SignKey,
                        setting.Value.Issuer,
                        setting.Value.ExpirationMinutes
                    );

                    // Return
                    return sqlClientBuilder;
                });
            }
        }


        // Class
        public class SettingDictionary : Dictionary<string, Setting>
        {

        }

        public class Setting
        {
            // Properties
            public string Algorithm { get; set; } = string.Empty;

            public string SignKey { get; set; } = string.Empty;

            public string Issuer { get; set; } = null;

            public int ExpirationMinutes { get; set; } = 30;
        }
    }
}
