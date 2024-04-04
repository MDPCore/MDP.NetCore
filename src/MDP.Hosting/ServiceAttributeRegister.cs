using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public partial class ServiceAttributeRegister
    {
        // Methods
        public static IServiceCollection RegisterModule(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // InstanceTypeList
            var instanceTypeList = MDP.Reflection.Type.FindAllApplicationType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // InstanceTypeList.Filter
            instanceTypeList = instanceTypeList.AsParallel().Where(instanceType =>
            {
                // Require
                if (instanceType.IsClass == false) return false;
                if (instanceType.IsPublic == false) return false;
                if (instanceType.IsAbstract == true) return false;
                if (instanceType.IsGenericType == true) return false;
                if (instanceType.IsDefined(typeof(ServiceAttribute), inherit: false) == false) return false;

                // Return
                return true;
            }).ToList();

            // InstanceTypeList.Foreach
            foreach (var instanceType in instanceTypeList)
            {
                // ServiceAttribute
                var serviceAttribute = instanceType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(ServiceAttribute<>)).OfType<ServiceAttribute>().FirstOrDefault();
                if (serviceAttribute == null) continue;

                // ServiceAttributeConfigList
                var serviceAttributeConfigList = new List<IConfigurationSection>();
                if (serviceAttribute.ServiceType.Namespace == instanceType.Namespace)
                {
                    // Add
                    serviceAttributeConfigList.AddRange(FindAllServiceAttributeConfig(configuration, instanceType.Namespace));
                }
                else
                {
                    // Add
                    serviceAttributeConfigList.AddRange(FindAllServiceAttributeConfig(configuration, instanceType.Namespace));
                    serviceAttributeConfigList.AddRange(FindAllServiceAttributeConfig(configuration, serviceAttribute.ServiceType.Namespace));
                }

                // ServiceAttribute.RegisterService
                var isRegistered = false;
                foreach (var serviceAttributeConfig in serviceAttributeConfigList)
                {
                    // InstanceName
                    var instanceName = serviceAttributeConfig.Key;
                    if (string.IsNullOrEmpty(instanceName) == true) throw new InvalidOperationException($"{nameof(instanceName)}=null");
                    if (instanceName.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == false) continue;

                    // RegisterService
                    ServiceRegister.RegisterService
                    (
                        serviceCollection: serviceCollection,
                        serviceType: serviceAttribute.ServiceType,
                        instanceType: instanceType,
                        instanceName: instanceName,
                        parameterProvider: new ConfigurationParameterProvider(serviceAttributeConfig),
                        singleton: serviceAttribute.Singleton
                    );

                    // IsRegistered
                    isRegistered = true;
                }

                // ServiceAttribute.AutoRegister
                if (isRegistered == false && serviceAttribute.AutoRegister == true)
                {
                    // RegisterService
                    ServiceRegister.RegisterService
                    (
                        serviceCollection: serviceCollection,
                        serviceType: serviceAttribute.ServiceType,
                        instanceType: instanceType,
                        instanceName: instanceType.Name,
                        parameterProvider: new DefaultParameterProvider(),
                        singleton: serviceAttribute.Singleton
                    );
                }
            }

            // Return
            return serviceCollection;
        }
    }

    public partial class ServiceAttributeRegister
    {
        // Methods
        private static List<IConfigurationSection> FindAllServiceAttributeConfig(IConfiguration configuration, string serviceNamespace)
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
