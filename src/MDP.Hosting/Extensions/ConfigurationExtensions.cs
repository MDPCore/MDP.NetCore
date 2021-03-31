using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLK.Configuration;

namespace MDP
{
    public static partial class ConfigurationExtensions
    {
        // Constants
        private const string ServiceTypeKey = "Type";

        private const string ServiceConnectionStringNameKey = "ConnectionString";


        // Methods
        public static string GetServiceType<TService>(this IConfiguration configuration) 
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            
            #endregion

            // Return
            return configuration.GetServiceValue<TService, string>(ServiceTypeKey);
        }

        public static string GetServiceConnectionString<TService>(this IConfiguration configuration) 
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ServiceConnectionStringName
            var serviceConnectionStringName = configuration.GetServiceValue<TService, string>(ServiceConnectionStringNameKey);
            if (string.IsNullOrEmpty(serviceConnectionStringName) == true) throw new InvalidOperationException($"{nameof(serviceConnectionStringName)}=null");

            // Return
            return configuration.GetConnectionString(serviceConnectionStringName);
        }
    }

    public static partial class ConfigurationExtensions
    {
        // Methods
        public static TValue GetServiceValue<TService, TValue>(this IConfiguration configuration, string key)
           where TService : class
           where TValue : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));

            #endregion

            // Return
            return configuration.GetServiceSection<TService>().GetValue<TValue>(key);
        }

        public static TSetting GetServiceSetting<TService, TSetting>(this IConfiguration configuration)
            where TService : class
            where TSetting : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Return
            return configuration.GetServiceSection<TService>().Bind<TSetting>();
        }

        public static IConfiguration GetServiceSection<TService>(this IConfiguration configuration)
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ServiceType
            var serviceType = typeof(TService);
            if (serviceType == null) throw new InvalidOperationException($"{nameof(serviceType)}=null");

            // ServiceSectionKey
            var serviceSectionKey = $"{serviceType.Namespace}:{serviceType.Name}";
            if (string.IsNullOrEmpty(serviceSectionKey) == true) throw new InvalidOperationException($"{nameof(serviceSectionKey)}=null");

            // ServiceSection
            var serviceSection = configuration.GetSection(serviceSectionKey);
            if (serviceSection == null) throw new InvalidOperationException($"{nameof(serviceSection)}=null");

            // Return
            return serviceSection;
        }
    }
}