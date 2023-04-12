using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Text.Json;

namespace MDP.Configuration
{
    public static class ConfigurationExtensions
    {
        // Methods   
        public static T? Bind<T>(this IConfigurationRoot configuration, string path) where T : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException($"{nameof(path)}=null");

            #endregion

            // Return
            return configuration.GetSection(path).Bind<T>();
        }

        public static T? Bind<T>(this IConfigurationSection configuration, string path) where T : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException($"{nameof(path)}=null");

            #endregion

            // Require
            if (configuration.Exists() == false) return null;

            // Return
            return configuration.GetSection(path).Bind<T>();
        }

        public static T? Bind<T>(this IConfigurationSection configuration) where T : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // Require
            if (configuration.Exists() == false) return null;

            // Bind
            var instance = new T();
            ConfigurationBinder.Bind(configuration, instance);

            // Return
            return instance;
        }
    }
}
