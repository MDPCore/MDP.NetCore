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
        private const string ImplementerNameKey = "Implementer";

        private const string ConnectionStringNameKey = "ConnectionString";

        private const string ConnectionStringNameDefault = "DefaultConnection";


        // Methods
        public static string GetImplementer<TService>(this IConfiguration configuration) 
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            
            #endregion

            // Return
            return configuration.GetValue<TService, string>(ImplementerNameKey);
        }

        public static string GetConnectionString<TService>(this IConfiguration configuration) 
            where TService : class
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ConnectionStringName
            var connectionStringName = configuration.GetValue<TService, string>(ConnectionStringNameKey);
            if (string.IsNullOrEmpty(connectionStringName) == true) connectionStringName = ConnectionStringNameDefault;
            if (string.IsNullOrEmpty(connectionStringName) == true) throw new InvalidOperationException($"{nameof(connectionStringName)}=null");

            // Return
            return configuration.GetConnectionString(connectionStringName);
        }
    }

    public static partial class ConfigurationExtensions
    {
        // Methods
        public static TValue GetValue<TService, TValue>(this IConfiguration configuration, string key)
           where TService : class
           where TValue : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));

            #endregion

            // Return
            return configuration.GetSection<TService>().GetValue<TValue>(key);
        }

        public static TSetting GetSetting<TService, TSetting>(this IConfiguration configuration)
            where TService : class
            where TSetting : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Return
            return configuration.GetSection<TService>().Bind<TSetting>();
        }

        public static IConfiguration GetSection<TService>(this IConfiguration configuration)
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