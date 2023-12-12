using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public partial class ServiceRegister
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

            // RegisterService
            foreach (var instanceType in instanceTypeList)
            {
                // Require
                if (instanceType.IsClass == false) continue;
                if (instanceType.IsPublic == false) continue;
                if (instanceType.IsAbstract == true) continue;
                if (instanceType.IsGenericType == true) continue;
                if (instanceType.FullName.StartsWith("Microsoft") == true) continue;
                if (instanceType.FullName.StartsWith("System") == true) continue;

                // ServiceAttribute
                var serviceAttribute = instanceType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(ServiceAttribute<>)).OfType<ServiceAttribute>().FirstOrDefault();
                if (serviceAttribute == null) continue;

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

                // RegisterService
                foreach (var instanceConfig in instanceConfigList)
                {
                    // InstanceName
                    var instanceName = instanceConfig.Key;
                    if (string.IsNullOrEmpty(instanceName) == true) throw new InvalidOperationException($"{nameof(instanceName)}=null");
                    if (instanceName.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == false) continue;

                    // RegisterService
                    ServiceRegister.RegisterService
                    (
                        containerBuilder: containerBuilder,
                        serviceType: serviceAttribute.ServiceType,
                        instanceType: instanceType,
                        instanceName: instanceName,
                        instanceConfig: instanceConfig,
                        singleton: serviceAttribute.Singleton
                    );
                }
            }

            // Return
            return containerBuilder;
        }

        public static void RegisterService(IServiceCollection containerBuilder, Type serviceType, Type instanceType, string instanceName, IConfigurationSection instanceConfig, bool singleton)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");

            #endregion

            // RegisterService
            ServiceRegister.RegisterService
            (
                containerBuilder: containerBuilder,
                serviceType: serviceType,
                instanceType: instanceType,
                instanceName: instanceName,
                parameterProvider: new ConfigurationParameterProvider(instanceConfig),
                singleton: singleton
            );
        }

        public static void RegisterService(IServiceCollection containerBuilder, Type serviceType, Type instanceType, string instanceName, Dictionary<string, object> parameters, bool singleton)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (parameters == null) throw new ArgumentException($"{nameof(parameters)}=null");

            #endregion

            // RegisterService
            ServiceRegister.RegisterService
            (
                containerBuilder: containerBuilder,
                serviceType: serviceType,
                instanceType: instanceType,
                instanceName: instanceName,
                parameterProvider: new DictionaryParameterProvider(parameters),
                singleton: singleton
            );
        }

        private static void RegisterService(IServiceCollection containerBuilder, Type serviceType, Type instanceType, string instanceName, ParameterProvider parameterProvider, bool singleton)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (parameterProvider == null) throw new ArgumentException($"{nameof(parameterProvider)}=null");

            #endregion

            // InstanceName
            if (instanceName.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == false) throw new InvalidOperationException($"{nameof(instanceName)}={instanceType.Name}");

            // FullInstanceName
            var fullInstanceName = $"{instanceType.Namespace}.{instanceName}";
            if (string.IsNullOrEmpty(fullInstanceName) == true) throw new InvalidOperationException($"{nameof(fullInstanceName)}=null");

            // RegisterTyped: ServiceType
            containerBuilder.RegisterTyped(serviceType, (serviceProvider) =>
            {
                return serviceProvider.ResolveNamed(serviceType, fullInstanceName);
            }
            , singleton);

            // RegisterNamed: InstanceName
            containerBuilder.RegisterNamed(serviceType, instanceName, (serviceProvider) =>
            {
                return serviceProvider.ResolveNamed(serviceType, fullInstanceName);
            }
            , singleton);

            // RegisterNamed: FullInstanceName
            containerBuilder.RegisterNamed(serviceType, fullInstanceName, (serviceProvider) =>
            {
                return ServiceActivator.CreateInstance(instanceType, parameterProvider, serviceProvider);
            }
            , singleton);
        }
    }

    public partial class ServiceRegister
    {
        // Methods
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
