using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.NetCore
{
    public class ServiceRegisterContext
    {
        // Methods
        public void RegisterModule(IServiceCollection containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // InstanceTypeList
            var instanceTypeList = CLK.Reflection.Type.FindAllType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // InstanceType
            foreach (var instanceType in instanceTypeList)
            {
                // RegisterInstance
                this.RegisterInstance(containerBuilder, configuration, instanceType);
            }
        }

        private void RegisterInstance(IServiceCollection containerBuilder, IConfiguration configuration, Type instanceType)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");

            #endregion

            // ServiceAttribute
            var serviceAttribute = instanceType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(ServiceAttribute<>)).OfType<ServiceAttribute>().FirstOrDefault();
            if (serviceAttribute == null) return;

            // InstanceConfigList
            var instanceConfigList = new List<IConfigurationSection>();
            instanceConfigList.AddRange(this.FindAllInstanceConfig(configuration, instanceType.Namespace!));
            instanceConfigList.AddRange(this.FindAllInstanceConfig(configuration, serviceAttribute.ServiceType.Namespace!));

            // InstanceConfig
            foreach (var instanceConfig in instanceConfigList)
            {
                // InstanceName
                var instanceName = $"{instanceConfig.Key}";
                if (string.IsNullOrEmpty(instanceName) == true) throw new InvalidOperationException($"{nameof(instanceName)}=null");

                // FullInstanceName
                var fullInstanceName = $"{instanceType.Namespace!}.{instanceConfig.Key}";
                if (string.IsNullOrEmpty(fullInstanceName) == true) throw new InvalidOperationException($"{nameof(fullInstanceName)}=null");

                // RegisterTyped
                if (instanceName.Equals(instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Default
                    containerBuilder.RegisterTyped(instanceType, (serviceProvider) =>
                    {
                        return ServiceActivator.CreateInstance(instanceType, instanceConfig, serviceProvider);
                    }
                    , serviceAttribute.Singleton);

                    if (instanceType != serviceAttribute.ServiceType)
                    {
                        containerBuilder.RegisterTyped(serviceAttribute.ServiceType, (serviceProvider) =>
                        {
                            return serviceProvider.ResolveTyped(instanceType);
                        }
                        , serviceAttribute.Singleton);
                    }

                    // InstanceName
                    containerBuilder.RegisterNamed(serviceAttribute.ServiceType, instanceName, (serviceProvider) =>
                    {
                        return serviceProvider.ResolveTyped(instanceType);
                    }
                    , serviceAttribute.Singleton);

                    // FullInstanceName
                    containerBuilder.RegisterNamed(serviceAttribute.ServiceType, fullInstanceName, (serviceProvider) =>
                    {
                        return serviceProvider.ResolveTyped(instanceType);
                    }
                    , serviceAttribute.Singleton);

                    // Continue
                    continue;
                }

                // RegisterNamed
                if (instanceName.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // InstanceName
                    containerBuilder.RegisterNamed(serviceAttribute.ServiceType, instanceName, (serviceProvider) =>
                    {
                        return ServiceActivator.CreateInstance(instanceType, instanceConfig, serviceProvider);
                    }
                    , serviceAttribute.Singleton);

                    // FullInstanceName
                    containerBuilder.RegisterNamed(serviceAttribute.ServiceType, fullInstanceName, (serviceProvider) =>
                    {
                        return serviceProvider.ResolveNamed(serviceAttribute.ServiceType, instanceName);
                    }
                    , serviceAttribute.Singleton);

                    // Continue
                    continue;
                }
            }
        }

        private List<IConfigurationSection> FindAllInstanceConfig(IConfiguration configuration, string serviceNamespace)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");

            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return new List<IConfigurationSection>();
            if (namespaceConfig.Exists() == false) return new List<IConfigurationSection>();

            // InstanceConfigList
            var instanceConfigList = namespaceConfig.GetChildren();
            if (instanceConfigList == null) throw new InvalidOperationException($"{nameof(instanceConfigList)}=null");

            // Return
            return instanceConfigList.ToList();
        }
    }
}
