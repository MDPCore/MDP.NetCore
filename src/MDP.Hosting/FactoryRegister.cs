using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public partial class FactoryRegister
    {
        // Methods
        public static TContainerBuilder RegisterModule<TContainerBuilder>(TContainerBuilder containerBuilder, IConfiguration configuration) where TContainerBuilder : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
           
            #endregion

            // FactoryTypeList
            var factoryTypeList = CLK.Reflection.Type.FindAllType();
            if (factoryTypeList == null) throw new InvalidOperationException($"{nameof(factoryTypeList)}=null");

            // FactoryType
            foreach (var factoryType in factoryTypeList)
            {
                // Require
                if (factoryType.IsClass == false) continue;
                if (factoryType.IsPublic == false) continue;
                if (factoryType.IsAbstract == true) continue;
                if (factoryType.IsGenericType == true) continue;
                if (factoryType.FullName.StartsWith("Microsoft") == true) continue;
                if (factoryType.FullName.StartsWith("System") == true) continue;
                if (factoryType.IsAssignableTo(typeof(Factory)) == false) continue;

                // FactoryInterface
                Type factoryInterface = factoryType;
                while (true)
                {
                    // BaseType
                    factoryInterface = factoryInterface.BaseType;
                    if (factoryInterface == typeof(object)) factoryInterface = null;
                    if (factoryInterface == null) break;

                    // Factory
                    if (factoryInterface.IsGenericType && factoryInterface.GetGenericTypeDefinition() == typeof(Factory<,>))
                    {
                        break;
                    }
                }
                if (factoryInterface == null) throw new InvalidOperationException($"{nameof(factoryInterface)}=null");

                // FactoryArguments
                var factoryArguments = factoryInterface.GetGenericArguments();
                if (factoryArguments.Length != 2) throw new InvalidOperationException($"{nameof(factoryArguments.Length)}={factoryArguments.Length}");
                if (factoryArguments[0].IsAssignableFrom(typeof(TContainerBuilder)) == false) continue;

                // SettingType
                var settingType = factoryArguments[1];

                // Factory
                var factory = Activator.CreateInstance(factoryType) as Factory;
                if (factory == null) throw new InvalidOperationException($"{nameof(factory)}=null");

                // FactoryConfig
                IConfigurationSection factoryConfig = null;
                if (string.IsNullOrEmpty(factory.ServiceName) == true)
                {
                    factoryConfig = FindNamespaceConfig(configuration, factory.ServiceNamespace);
                }
                else
                {
                    factoryConfig = FindServiceConfig(configuration, factory.ServiceNamespace, factory.ServiceName);
                }
                if (factoryConfig == null) continue;


                // RegisterFactory
                RegisterFactory(containerBuilder, factory, settingType, factoryConfig);
            }

            // Return
            return containerBuilder;
        }

        private static void RegisterFactory<TContainerBuilder>(TContainerBuilder containerBuilder, Factory factory, Type settingType, IConfigurationSection factoryConfig) where TContainerBuilder : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (factory == null) throw new ArgumentException($"{nameof(factory)}=null");
            if (settingType == null) throw new ArgumentException($"{nameof(settingType)}=null");
            if (factoryConfig == null) throw new ArgumentException($"{nameof(factoryConfig)}=null");

            #endregion           

            // FactoryMethod
            var factoryMethod = factory.GetType().GetMethod("ConfigureService");
            if (factoryMethod == null) throw new InvalidOperationException($"Factory.ConfigureService({typeof(TContainerBuilder)}, {settingType}) not found.");

            // ParameterList
            var parameterList = factoryMethod.GetParameters();
            if (parameterList == null) throw new InvalidOperationException($"{nameof(parameterList)}=null");
            if (parameterList.Length != 2) throw new InvalidOperationException($"Factory.RegisterService({typeof(TContainerBuilder)}, {settingType}) not found.");
            if (parameterList[0].ParameterType.IsAssignableFrom(typeof(TContainerBuilder)) == false) throw new InvalidOperationException($"Factory.RegisterService({typeof(TContainerBuilder)}, {settingType}) not found.");
            if (parameterList[1].ParameterType.IsAssignableFrom(settingType) == false) throw new InvalidOperationException($"Factory.RegisterService({typeof(TContainerBuilder)}, {settingType}) not found.");

            // FactorySetting
            var factorySetting = Activator.CreateInstance(settingType);
            if (factorySetting == null) throw new InvalidOperationException($"{nameof(factorySetting)}=null");
            ConfigurationBinder.Bind(factoryConfig, factorySetting);

            // Invoke
            factoryMethod.Invoke(factory, new object[] { containerBuilder, factorySetting });
        }
    }

    public partial class FactoryRegister
    {
        // Methods
        private static IConfigurationSection FindNamespaceConfig(IConfiguration configuration, string serviceNamespace)
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

        private static IConfigurationSection FindServiceConfig(IConfiguration configuration, string serviceNamespace, string serviceName)
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

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Return
            return serviceConfigList.FirstOrDefault(o => string.Equals(o.Key, serviceName, StringComparison.OrdinalIgnoreCase));
        }
    }
}