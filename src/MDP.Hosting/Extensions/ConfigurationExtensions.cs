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
        private const string ServiceNameKey = "Name";


        // Methods
        public static string GetServiceName<TService>(this IConfiguration configuration) where TService : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            
            #endregion

            // Return
            return configuration.GetSection<TService>().GetValue<string>(ServiceNameKey);
        }

        public static TSetting GetServiceSetting<TService, TSetting>(this IConfiguration configuration) where TService : notnull where TSetting : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Return
            return configuration.GetSection<TService>().Bind<TSetting>();
        }
    }

    public static partial class ConfigurationExtensions
    {
        // Methods
        public static IConfiguration GetSection<TService>(this IConfiguration configuration) where TService : notnull
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