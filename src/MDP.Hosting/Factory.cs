using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Factory<TService>
        where TService : class
    {
        // Methods
        internal abstract TService Create(IComponentContext componentContext, string serviceName = null);

        internal IConfigurationSection CreateServiceConfig(IComponentContext componentContext, string serviceName = null)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion
                        
            // NamespaceConfigKey
            var namespaceConfigKey = typeof(TService).Namespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new InvalidOperationException($"{nameof(namespaceConfigKey)}=null");

            // ServiceConfigKey
            var serviceConfigKey = this.GetType().Name;
            if (string.IsNullOrEmpty(serviceConfigKey) == true)
            {
                throw new InvalidOperationException($"{nameof(serviceConfigKey)}=null");
            }
            if (serviceConfigKey.EndsWith("Factory") == true)
            {
                serviceConfigKey = serviceConfigKey.Substring(0, serviceConfigKey.Length - "Factory".Length);
            }
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new InvalidOperationException($"{serviceConfigKey}=null");

            // ServiceName
            if (string.IsNullOrEmpty(serviceName) == false)
            {
                // Require
                if (serviceName.StartsWith(serviceConfigKey) == false) return null;

                // Replace
                serviceConfigKey = serviceName;
            }
           
            // ConfigRoot
            var configRoot = componentContext.Resolve<IConfiguration>() as ConfigurationRoot;
            if (configRoot == null) throw new InvalidOperationException($"{nameof(configRoot)}=null");

            // NamespaceConfig
            var namespaceConfig = configRoot.GetSection(namespaceConfigKey);
            if (namespaceConfig == null) return null;

            // ServiceConfig
            var serviceConfig = namespaceConfig.GetSection(serviceConfigKey);
            if (serviceConfig == null) return null;
            if (serviceConfig.Exists() == true) return serviceConfig;
            if (this.ExistsJsonServiceConfig(configRoot, namespaceConfigKey, serviceConfigKey) == true) return serviceConfig;

            // Return
            return null;
        }

        private bool ExistsJsonServiceConfig(ConfigurationRoot configRoot, string namespaceConfigKey, string serviceConfigKey)
        {
            #region Contracts

            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new ArgumentException(nameof(namespaceConfigKey));
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new ArgumentException(nameof(serviceConfigKey));

            #endregion

            // JsonSource
            foreach (var jsonSource in configRoot.Providers.OfType<JsonConfigurationProvider>().Select(o => o.Source))
            {
                // JsonFile
                var jsonFile = jsonSource.FileProvider?.GetFileInfo(jsonSource.Path);
                if (jsonFile == null) continue;
                if (jsonFile.Exists == false) continue;

                // JsonDocumentOptions
                var jsonDocumentOptions = new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                };

                // JsonDocument
                using (var jsonStream = jsonFile.CreateReadStream())
                using (var jsonDocument = JsonDocument.Parse(jsonStream, jsonDocumentOptions))
                {
                    // JsonElement
                    if (jsonDocument.RootElement.TryGetProperty(namespaceConfigKey, out var namespaceConfigElement) == false) continue;
                    if (namespaceConfigElement.TryGetProperty(serviceConfigKey, out var serviceConfigElement) == false) continue;

                    // Return
                    return true;
                }
            }

            // Return
            return false;
        }
    }

    public abstract class Factory<TService, TInstance, TSetting> : Factory<TService>
        where TService : class
        where TInstance : TService
        where TSetting : class, new()
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

            // ServiceSetting
            var serviceSetting = new TSetting();
            ConfigurationBinder.Bind(serviceConfig, serviceSetting);

            // CreateService
            return this.CreateService(componentContext, serviceSetting);
        }

        protected abstract TInstance CreateService(IComponentContext componentContext, TSetting setting);
    }
}