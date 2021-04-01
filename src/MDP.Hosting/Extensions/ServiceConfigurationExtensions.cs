using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    public static partial class ServiceConfigurationExtensions
    {
        // Constants
        private const string ImplementerNameKey = "Implementer";

        private const string ConnectionStringNameKey = "ConnectionString";

        private const string ConnectionStringNameDefault = "DefaultConnection";


        // Methods
        public static string GetImplementerName(this IServiceConfiguration configuration) 
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            
            #endregion

            // Return
            return configuration.ServiceSection.GetValue<string>(ImplementerNameKey);
        }

        public static string GetConnectionString(this IServiceConfiguration configuration) 
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // ConnectionStringName
            var connectionStringName = configuration.ServiceSection.GetValue<string>(ConnectionStringNameKey);
            if (string.IsNullOrEmpty(connectionStringName) == true) connectionStringName = ConnectionStringNameDefault;
            if (string.IsNullOrEmpty(connectionStringName) == true) throw new InvalidOperationException($"{nameof(connectionStringName)}=null");

            // Return
            return configuration.RootSection.GetConnectionString(connectionStringName);
        }
    }

    public static partial class ServiceConfigurationExtensions
    {
        // Methods
        public static TValue GetValue<TValue>(this IServiceConfiguration configuration, string key)
           where TValue : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));

            #endregion

            // Return
            return configuration.ServiceSection.GetValue<TValue>(key);
        }

        public static TSetting GetSetting<TSetting>(this IServiceConfiguration configuration)
            where TSetting : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Return
            return configuration.ServiceSection.Get<TSetting>();
        }

        public static TSetting GetSetting<TSetting>(this IServiceConfiguration configuration, string key)
            where TSetting : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));

            #endregion

            // Return
            return configuration.ServiceSection.GetSection(key).Get<TSetting>();
        }
    }
}