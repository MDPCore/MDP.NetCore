using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Factory<TService>
        where TService : class
    {
        // Methods
        internal abstract TService Create(IComponentContext componentContext, string serviceName = null);
    }

    public abstract class Factory<TService, TInstance> : Factory<TService>
        where TService : class
        where TInstance : TService
    {
        // Methods
        internal override TService Create(IComponentContext componentContext, string serviceName = null)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // ServiceConfig
            var serviceConfig = this.CreateServiceConfig(componentContext, serviceName);
            if (serviceConfig == null) return null;

            // Bind
            ConfigurationBinder.Bind(serviceConfig, this);

            // CreateService
            return this.CreateService(componentContext);
        }

        protected abstract TInstance CreateService(IComponentContext componentContext);

        private IConfigurationSection CreateServiceConfig(IComponentContext componentContext, string serviceName = null)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // ConfigRoot
            var configRoot = componentContext.Resolve<IConfiguration>();
            if (configRoot == null) throw new InvalidOperationException($"{nameof(configRoot)}=null");

            // NamespaceConfig
            var namespaceConfig = configRoot.GetSection(typeof(TService).Namespace);
            if (namespaceConfig == null) return null;

            // ServiceConfigKey
            var serviceConfigKey = this.GetType().Name;
            if (serviceConfigKey.EndsWith("Factory") == true)
            {
                serviceConfigKey = serviceConfigKey.Substring(0, serviceConfigKey.Length - "Factory".Length);
            }
            if (string.IsNullOrEmpty(serviceName) == false)
            {
                serviceConfigKey += $"[{serviceName}]";
            }
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new InvalidOperationException($"{serviceConfigKey}=null");

            // ServiceConfig
            var serviceConfig = namespaceConfig.GetSection(serviceConfigKey);
            if (serviceConfig == null) return null;
            if (serviceConfig.Exists() == false) return null;

            // Return
            return serviceConfig;
        }
    }
}