using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public partial class ServiceFactoryRegister
    {
        // Methods
        public static void RegisterModule<TBuilder>(TBuilder builder, IConfiguration configuration) where TBuilder : class
        {
            #region Contracts

            if (builder == null) throw new ArgumentException($"{nameof(builder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
           
            #endregion

            // ServiceFactoryTypeList
            var serviceFactoryTypeList = CLK.Reflection.Type.FindAllType();
            if (serviceFactoryTypeList == null) throw new InvalidOperationException($"{nameof(serviceFactoryTypeList)}=null");

            // ServiceFactoryType
            foreach (var serviceFactoryType in serviceFactoryTypeList)
            {
                // Require
                if (serviceFactoryType.IsClass == false) continue;
                if (serviceFactoryType.IsPublic == false) continue;
                if (serviceFactoryType.IsAbstract == true) continue;
                if (serviceFactoryType.IsGenericType == true) continue;
                if (serviceFactoryType.FullName.StartsWith("Microsoft") == true) continue;
                if (serviceFactoryType.FullName.StartsWith("System") == true) continue;
                if (serviceFactoryType.IsAssignableTo(typeof(ServiceFactory)) == false) continue;

                // ServiceFactoryInterface
                var serviceFactoryInterface = serviceFactoryType;
                while (true)
                {
                    // BaseType
                    serviceFactoryInterface = serviceFactoryInterface.BaseType;
                    if (serviceFactoryInterface == typeof(object)) serviceFactoryInterface = null;
                    if (serviceFactoryInterface == null) break;
                    if (serviceFactoryInterface.IsGenericType && serviceFactoryInterface.GetGenericTypeDefinition() == typeof(ServiceFactory<>)) break;
                    if (serviceFactoryInterface.IsGenericType && serviceFactoryInterface.GetGenericTypeDefinition() == typeof(ServiceFactory<,>)) break;
                }
                if (serviceFactoryInterface == null) throw new InvalidOperationException($"{nameof(serviceFactoryInterface)}=null");

                // ServiceFactoryArguments
                var serviceFactoryArguments = serviceFactoryInterface.GetGenericArguments();
                if (serviceFactoryArguments[0].IsAssignableFrom(typeof(TBuilder)) == false) continue;

                // ServiceFactory
                var serviceFactory = Activator.CreateInstance(serviceFactoryType) as ServiceFactory;
                if (serviceFactory == null) throw new InvalidOperationException($"{nameof(serviceFactory)}=null");

                // SettingConfig
                IConfigurationSection settingConfig = null;
                if (string.IsNullOrEmpty(serviceFactory.ServiceName) == true)
                {
                    settingConfig = FindNamespaceConfig(configuration, serviceFactory.ServiceNamespace);
                }
                else
                {
                    settingConfig = FindServiceConfig(configuration, serviceFactory.ServiceNamespace, serviceFactory.ServiceName);
                }
                if (settingConfig == null) continue;

                // Setting
                object setting = null;
                if (serviceFactoryArguments.Length == 2)
                {
                    // SettingType
                    var settingType = serviceFactoryArguments[1];
                    if (settingType == null) throw new InvalidOperationException($"{nameof(settingType)}=null");

                    // Setting
                    setting = Activator.CreateInstance(settingType);
                    if (setting == null) throw new InvalidOperationException($"{nameof(setting)}=null");

                    // Bind
                    ConfigurationBinder.Bind(settingConfig, setting);
                }

                // ServiceFactory.ConfigureService
                {
                    // ConfigureServiceMethod
                    var configureServiceMethod = serviceFactory.GetType().GetMethod("ConfigureService");
                    if (configureServiceMethod == null) throw new InvalidOperationException($"Factory.ConfigureService({typeof(TBuilder)}, {setting.GetType()}) not found.");

                    // ConfigureServiceParameters
                    var configureServiceParameters = configureServiceMethod.GetParameters();
                    if (configureServiceParameters == null) throw new InvalidOperationException($"{nameof(configureServiceParameters)}=null");

                    // Invoke
                    if (configureServiceParameters.Length == 1) configureServiceMethod.Invoke(serviceFactory, new object[] { builder });
                    if (configureServiceParameters.Length == 2) configureServiceMethod.Invoke(serviceFactory, new object[] { builder, setting });
                }
            }
        }
    }

    public partial class ServiceFactoryRegister
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