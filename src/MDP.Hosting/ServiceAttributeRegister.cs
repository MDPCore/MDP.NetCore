using MDP.Registration;
using CLK.ComponentModel;
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
            var instanceTypeList = CLK.Reflection.Type.FindAllType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // InstanceType
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
                    ServiceAttributeRegister.RegisterService
                    (
                        serviceCollection: serviceCollection,
                        serviceType: serviceAttribute.ServiceType,
                        instanceType: instanceType,
                        instanceName: instanceName,
                        parameterProvider: new ConfigurationParameterProvider(instanceConfig),
                        singleton: serviceAttribute.Singleton
                    );
                }
            }

            // Return
            return serviceCollection;
        }

        internal static void RegisterService(IServiceCollection serviceCollection, Type serviceType, Type instanceType, string instanceName, ParameterProvider parameterProvider, bool singleton)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
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
            serviceCollection.RegisterTyped(serviceType, (serviceProvider) =>
            {
                return serviceProvider.ResolveNamed(serviceType, fullInstanceName);
            }
            , singleton);

            // RegisterNamed: InstanceName
            serviceCollection.RegisterNamed(serviceType, instanceName, (serviceProvider) =>
            {
                return serviceProvider.ResolveNamed(serviceType, fullInstanceName);
            }
            , singleton);

            // RegisterNamed: FullInstanceName
            serviceCollection.RegisterNamed(serviceType, fullInstanceName, (serviceProvider) =>
            {
                // ConstructorInfo
                var constructorInfo = instanceType.GetConstructors().MaxBy(o => o.GetParameters().Length);
                if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

                // Parameters
                var parameters = new List<object>();
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    parameters.Add(parameterProvider.CreateParameter(parameterInfo, serviceProvider));
                }

                // Instance
                var instance = constructorInfo.Invoke(parameters.ToArray());
                if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                // Return
                return instance;
            }
            , singleton);
        }
    }

    public partial class ServiceAttributeRegister
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
