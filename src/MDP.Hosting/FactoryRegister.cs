using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public class FactoryRegister
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
                // RegisterFactory
                RegisterFactory(containerBuilder, configuration, factoryType);
            }

            // Return
            return containerBuilder;
        }

        private static void RegisterFactory<TContainerBuilder>(TContainerBuilder containerBuilder, IConfiguration configuration, Type factoryType) where TContainerBuilder : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (factoryType == null) throw new ArgumentException($"{nameof(factoryType)}=null");

            #endregion

            // FactoryAttribute
            var factoryAttribute = factoryType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(FactoryAttribute<,>)).OfType<FactoryAttribute>().FirstOrDefault();
            if (factoryAttribute == null) return;
            if (factoryAttribute.BuilderType != typeof(TContainerBuilder)) return;

            // ServiceName
            if (string.IsNullOrEmpty(factoryAttribute.ServiceName) == true)
            {
                // FactoryConfig
                var factoryConfig = FindNamespaceConfig(configuration, factoryAttribute.ServiceNamespace);
                if (factoryConfig == null) return;

                // InvokeFactory
                InvokeFactory(containerBuilder, factoryType, factoryAttribute, factoryConfig);
            }
            else
            {
                // FactoryConfigList
                var factoryConfigList = FindAllFactoryConfig(configuration, factoryAttribute.ServiceNamespace);
                if (factoryConfigList == null) throw new InvalidOperationException($"{nameof(factoryConfigList)}=null");

                // FactoryConfig
                foreach (var factoryConfig in factoryConfigList)
                {
                    if (factoryConfig.Key.StartsWith(factoryAttribute.ServiceName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // InvokeFactory
                        InvokeFactory(containerBuilder, factoryType, factoryAttribute, factoryConfig);
                    }
                }
            }
        }

        private static void InvokeFactory<TContainerBuilder>(TContainerBuilder containerBuilder, Type factoryType, FactoryAttribute factoryAttribute, IConfigurationSection factoryConfig) where TContainerBuilder : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (factoryType == null) throw new ArgumentException($"{nameof(factoryType)}=null");
            if (factoryAttribute == null) throw new ArgumentException($"{nameof(factoryAttribute)}=null");
            if (factoryConfig == null) throw new ArgumentException($"{nameof(factoryConfig)}=null");

            #endregion

            // Require
            if (factoryType.IsAbstract == true) return;

            // FactoryMethod
            var ractoryMethod = factoryType.GetMethod("ConfigureService");
            if (ractoryMethod == null) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");

            // ParameterList
            var parameterList = ractoryMethod.GetParameters();
            if (parameterList == null) throw new InvalidOperationException($"{nameof(parameterList)}=null");
            if (parameterList.Length != 2) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");
            if (parameterList[0].ParameterType != typeof(TContainerBuilder)) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");
            if (parameterList[1].ParameterType != factoryAttribute.SettingType) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");

            // FactorySetting
            var factorySetting = Activator.CreateInstance(factoryAttribute.SettingType);
            if (factorySetting == null) throw new InvalidOperationException($"{nameof(factorySetting)}=null");
            ConfigurationBinder.Bind(factoryConfig, factorySetting);

            // Factory
            var factory = Activator.CreateInstance(factoryType);
            if (factory == null) throw new InvalidOperationException($"{nameof(factory)}=null");

            // Invoke
            ractoryMethod.Invoke(factory, new object[] { containerBuilder, factorySetting });
        }

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

        private static List<IConfigurationSection> FindAllFactoryConfig(IConfiguration configuration, string serviceNamespace)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");

            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return new List<IConfigurationSection>();
            if (namespaceConfig.Exists() == false) return new List<IConfigurationSection>();

            // FactoryConfigList
            var factoryConfigList = namespaceConfig.GetChildren();
            if (factoryConfigList == null) throw new InvalidOperationException($"{nameof(factoryConfigList)}=null");

            // Return
            return factoryConfigList.ToList();
        }
    }
}