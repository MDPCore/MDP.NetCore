using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    internal static partial class ConfigurationExtensions
    {
        // Methods
        public static List<IConfigurationSection> GetChildren<TService, TOptions>(this IConfiguration configuration)
            where TService : class
            where TOptions : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ServiceConfigList
            var serviceConfigList = configuration.GetSection(typeof(TService).FullName)?.GetChildren()?.ToList();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Filter
            var filterCount = 0;
            foreach (var propertyInfo in typeof(TOptions).GetProperties())
            {
                // Remove
                filterCount += serviceConfigList.RemoveAll(serviceConfig => String.Equals(serviceConfig.Key, propertyInfo.Name, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Section
            if (filterCount > 0)
            {
                // ServiceConfig
                var serviceConfig = configuration.GetSection(typeof(TService).FullName);
                if (serviceConfig == null) throw new InvalidOperationException($"{nameof(serviceConfig)}=null");

                // Add
                serviceConfigList.Add(serviceConfig);
            }

            // Return
            return serviceConfigList;
        }

        public static List<IConfigurationSection> GetChildren<TService>(this IConfiguration configuration)
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ServiceConfigList
            var serviceConfigList = configuration.GetSection(typeof(TService).FullName)?.GetChildren()?.ToList();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Filter
            var filterCount = 0;
            foreach (var propertyInfo in typeof(TService).GetProperties())
            {
                // Remove
                filterCount += serviceConfigList.RemoveAll(serviceConfig => String.Equals(serviceConfig.Key, propertyInfo.Name, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Section
            if (filterCount > 0)
            {
                // ServiceConfig
                var serviceConfig = configuration.GetSection(typeof(TService).FullName);
                if (serviceConfig == null) throw new InvalidOperationException($"{nameof(serviceConfig)}=null");

                // Add
                serviceConfigList.Add(serviceConfig);
            }

            // Return
            return serviceConfigList;
        }
    }
}
