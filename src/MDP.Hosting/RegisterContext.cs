using Autofac;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract partial class RegisterContext
    {
        // Methods
        protected IConfigurationSection? FindServiceConfig(IConfiguration configuration, string serviceNamespace)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
           
            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return null;
            if (namespaceConfig.Exists() == false) return null;

            // Return
            return namespaceConfig;
        }

        protected IConfigurationSection? FindServiceConfig(IConfiguration configuration, string serviceNamespace, string serviceName)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
            if (string.IsNullOrEmpty(serviceName) == true) throw new ArgumentException($"{nameof(serviceName)}=null");

            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return null;
            if (namespaceConfig.Exists() == false) return null;

            // ServiceConfig
            var serviceConfig = namespaceConfig.GetSection(serviceName);
            if (serviceConfig == null) return null;
            if (serviceConfig.Exists() == false) return null;

            // Return
            return serviceConfig;
        }

        protected List<IConfigurationSection> FindAllServiceConfig(IConfiguration configuration, string serviceNamespace)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
          
            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return new List<IConfigurationSection>();
            if (namespaceConfig.Exists() == false) return new List<IConfigurationSection>();

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Return
            return serviceConfigList.ToList();
        }
    }
}