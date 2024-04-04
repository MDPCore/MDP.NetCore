using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
            var serviceFactoryTypeList = MDP.Reflection.Type.FindAllApplicationType();
            if (serviceFactoryTypeList == null) throw new InvalidOperationException($"{nameof(serviceFactoryTypeList)}=null");

            // ServiceFactoryTypeList.Filter
            serviceFactoryTypeList = serviceFactoryTypeList.AsParallel().Where(serviceFactoryType =>
            {
                // Require
                if (serviceFactoryType.IsClass == false) return false;
                if (serviceFactoryType.IsPublic == false) return false;
                if (serviceFactoryType.IsAbstract == true) return false;
                if (serviceFactoryType.IsGenericType == true) return false;
                if (serviceFactoryType.IsAssignableTo(typeof(ServiceFactory)) == false) return false;

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
                if (serviceFactoryArguments[0].IsAssignableFrom(typeof(TBuilder)) == false) return false;

                // Return
                return true;
            }).ToList();
        
            // ServiceFactoryType
            foreach (var serviceFactoryType in serviceFactoryTypeList)
            {
                // ServiceFactory
                var serviceFactory = Activator.CreateInstance(serviceFactoryType) as ServiceFactory;
                if (serviceFactory == null) throw new InvalidOperationException($"{nameof(serviceFactory)}=null");

                // ServiceFactoryConfig
                var serviceFactoryConfig = FindServiceFactoryConfig(configuration, serviceFactory.ServiceNamespace, serviceFactory.ServiceName);
                if (serviceFactoryConfig == null && serviceFactory.AutoRegister == false) continue;

                // ServiceFactory.ConfigureService
                {
                    // ConfigureServiceMethod
                    var configureServiceMethod = serviceFactory.GetType().GetMethod("ConfigureService");
                    if (configureServiceMethod == null) throw new InvalidOperationException($"Factory.ConfigureService not found.");

                    // ConfigureServiceParameters
                    var configureServiceParameters = configureServiceMethod.GetParameters();
                    if (configureServiceParameters == null) throw new InvalidOperationException($"{nameof(configureServiceParameters)}=null");
                    switch (configureServiceParameters.Length)
                    {
                        case 1: break;
                        case 2: break;
                        default: throw new InvalidOperationException($"{nameof(configureServiceParameters.Length)}={configureServiceParameters.Length}");
                    }

                    // ConfigureServiceParameters[0]
                    if (configureServiceParameters[0].ParameterType.IsAssignableFrom(typeof(TBuilder)) == false)
                    {
                        throw new InvalidOperationException($"configureServiceParameters[0].ParameterType!=typeof({typeof(TBuilder).Name})");
                    }
                    
                    // ConfigureServiceParameters[1]
                    object setting = null;
                    if (configureServiceParameters.Length == 2)
                    {
                        // SettingType
                        var settingType = configureServiceParameters[1].ParameterType;
                        if (settingType == null) throw new InvalidOperationException($"{nameof(settingType)}=null");

                        // Setting
                        setting = Activator.CreateInstance(settingType);
                        if (setting == null) throw new InvalidOperationException($"{nameof(setting)}=null");

                        // Bind
                        if (serviceFactoryConfig != null) ConfigurationBinder.Bind(serviceFactoryConfig, setting);
                    }

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
        private static IConfigurationSection FindServiceFactoryConfig(IConfiguration configuration, string serviceNamespace, string serviceName = null)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
            //if (string.IsNullOrEmpty(serviceName) == true) throw new ArgumentException($"{nameof(serviceName)}=null");

            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return null;
            if (namespaceConfig.Exists() == false) return null;
            if (string.IsNullOrEmpty(serviceName) == true) return namespaceConfig;

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren()?.FirstOrDefault(o => string.Equals(o.Key, serviceName, StringComparison.OrdinalIgnoreCase));
            if (serviceConfigList == null) return null;

            // Return
            return serviceConfigList;
        }
    }
}