using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public class ServiceRegister
    {
        // Methods
        public static IServiceCollection RegisterModule(IServiceCollection containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // InstanceTypeList
            var instanceTypeList = CLK.Reflection.Type.FindAllType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // RegisterInstance
            foreach (var instanceType in instanceTypeList)
            {
                RegisterInstance(containerBuilder, configuration, instanceType);
            }

            // Return
            return containerBuilder;
        }

        private static void RegisterInstance(IServiceCollection containerBuilder, IConfiguration configuration, Type instanceType)
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
            if (serviceAttribute.ServiceType.Namespace == instanceType.Namespace)
            {
                // Add
                instanceConfigList.AddRange(FindAllInstanceConfig(configuration, instanceType.Namespace));
            }
            else
            {
                // Add
                instanceConfigList.AddRange(FindAllInstanceConfig(configuration, instanceType.Namespace));
                instanceConfigList.AddRange(FindAllInstanceConfig(configuration, serviceAttribute.ServiceType.Namespace));
            }

            // RegisterInstance
            foreach (var instanceConfig in instanceConfigList)
            {
                // InstanceName
                var instanceName = instanceConfig.Key;
                if (string.IsNullOrEmpty(instanceName) == true) throw new InvalidOperationException($"{nameof(instanceName)}=null");
                if (instanceName.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == false) continue;

                // FullInstanceName
                var fullInstanceName = $"{instanceType.Namespace}.{instanceName}";
                if (string.IsNullOrEmpty(fullInstanceName) == true) throw new InvalidOperationException($"{nameof(fullInstanceName)}=null");

                // RegisterTyped: ServiceType
                containerBuilder.RegisterTyped(serviceAttribute.ServiceType, (serviceProvider) =>
                {
                    return serviceProvider.ResolveNamed(serviceAttribute.ServiceType, fullInstanceName);
                }
                , serviceAttribute.Singleton);

                // RegisterNamed: InstanceName
                containerBuilder.RegisterNamed(serviceAttribute.ServiceType, instanceName, (serviceProvider) =>
                {
                    return serviceProvider.ResolveNamed(serviceAttribute.ServiceType, fullInstanceName);
                }
                , serviceAttribute.Singleton);

                // RegisterNamed: FullInstanceName
                containerBuilder.RegisterNamed(serviceAttribute.ServiceType, fullInstanceName, (serviceProvider) =>
                {                    
                    return ServiceActivator.CreateInstance(instanceType, instanceConfig, serviceProvider);
                }
                , serviceAttribute.Singleton);
            }
        }

        private static List<IConfigurationSection> FindAllInstanceConfig(IConfiguration configuration, string serviceNamespace)
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
