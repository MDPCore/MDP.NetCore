using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    public static partial class ConfigurationExtensions
    {
        // Methods
        public static string GetServiceName<TService>(this IConfiguration configuration)
            where TService : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ServiceType
            var serviceType = typeof(TService);
            if (serviceType == null) throw new InvalidOperationException($"{nameof(serviceType)}=null");

            // ServiceNameKey
            var serviceNameKey = $"{serviceType.Namespace}:{serviceType.Name}:Name";
            if (string.IsNullOrEmpty(serviceNameKey) == true) throw new InvalidOperationException($"{nameof(serviceNameKey)}=null");

            // ServiceName
            var serviceName = configuration.GetValue<string>(serviceNameKey);
            if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

            // Return
            return serviceName;
        }
    }
}