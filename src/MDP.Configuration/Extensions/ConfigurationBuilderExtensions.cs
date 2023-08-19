using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Linq;

namespace MDP.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        // Methods   
        public static IConfigurationBuilder RegisterModule(this IConfigurationBuilder configurationBuilder, string environmentName, string configDirectoryName = "config")
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException($"{nameof(configurationBuilder)}=null");
            if (string.IsNullOrEmpty(environmentName) == true) throw new ArgumentException($"{nameof(environmentName)}=null");

            #endregion

            // ConfigurationRegister
            ConfigurationRegister.RegisterModule(configurationBuilder, environmentName, configDirectoryName);

            // Return
            return configurationBuilder;
        }
    }
}
