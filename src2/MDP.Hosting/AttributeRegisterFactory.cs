using Autofac;
using Autofac.Builder;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public class AttributeRegisterFactory : RegisterFactory<ContainerBuilder, IConfiguration>
    {
        // Constructors
        public AttributeRegisterFactory(Type serviceType, Type instanceType, string serviceNamespace, bool serviceSingleton)
        {
            #region Contracts

            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
           
            #endregion

            // Default
            this.ServiceType = serviceType;
            this.InstanceType = instanceType;
            this.ServiceNamespace = serviceNamespace;
            this.ServiceSingleton = serviceSingleton;
        }


        // Properties
        public Type ServiceType { get; private set; }

        public Type InstanceType { get; private set; }

        public string ServiceNamespace { get; private set; }

        public bool ServiceSingleton { get; private set; }


        // Methods   
        public void RegisterService(ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // NamespaceConfigKey            
            var namespaceConfigKey = this.ServiceNamespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new InvalidOperationException($"{nameof(namespaceConfigKey)}=null");

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(namespaceConfigKey);
            if (namespaceConfig == null) return;
            if (namespaceConfig.Exists() == false) return;

            // InstanceConfigKey
            var instanceConfigKey = this.InstanceType.Name;
            if (string.IsNullOrEmpty(instanceConfigKey) == true) throw new InvalidOperationException($"{nameof(instanceConfigKey)}=null");

            // InstanceConfigList
            var instanceConfigList = namespaceConfig.GetChildren();
            if (instanceConfigList == null) throw new InvalidOperationException($"{nameof(instanceConfigList)}=null");

            // InstanceConfig
            foreach (var instanceConfig in instanceConfigList)
            {
                // DefaultInstanceConfig
                if (string.Equals(instanceConfig.Key, instanceConfigKey, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // RegisterDefault
                    containerBuilder.Register((componentContext) =>
                    {
                        return componentContext.ResolveNamed(instanceConfig.Key, this.ServiceType);
                    })
                    .As(this.ServiceType);

                    // RegisterService
                    this.RegisterService(containerBuilder, instanceConfig);

                    // Continue
                    continue;
                }

                // NamedInstanceConfig
                if (instanceConfig.Key.StartsWith(instanceConfigKey, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // RegisterService
                    this.RegisterService(containerBuilder, instanceConfig);

                    // Continue
                    continue;
                }
            }
        }

        private void RegisterService(ContainerBuilder containerBuilder, IConfigurationSection instanceConfig)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            
            #endregion

            // Register
            var registration = containerBuilder.Register((componentContext) =>
            {
                // ConstructorInfo
                var constructorInfo = this.InstanceType.GetConstructors().FirstOrDefault();
                if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

                // ParameterList
                var parameterList = new List<object?>();
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    parameterList.Add(this.CreateParameter(parameterInfo, instanceConfig, componentContext));
                }

                // Instance
                var instance = constructorInfo.Invoke(parameterList.ToArray());
                if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                // Return
                return instance;
            });
            if (registration == null) throw new InvalidOperationException($"{nameof(registration)}=null");

            // As
            registration = registration.Named(instanceConfig.Key, this.ServiceType);

            // Singleton
            if (this.ServiceSingleton == true)
            {
                registration = registration.SingleInstance();
            }
        }

        private object? CreateParameter(ParameterInfo parameterInfo, IConfigurationSection instanceConfig, IComponentContext componentContext)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");

            #endregion

            // Primitive
            if (parameterInfo.ParameterType.IsPrimitive == true || parameterInfo.ParameterType == typeof(string))
            {
                // Parameter
                var parameter = instanceConfig.GetValue(parameterInfo.ParameterType, parameterInfo.Name);

                // Return
                return parameter;
            }

            // Resolve
            if (parameterInfo.ParameterType.IsInterface == true || parameterInfo.ParameterType.IsClass == true)
            {
                // InstanceName
                var instanceName = instanceConfig.GetValue<string>(parameterInfo.Name);

                // Instance
                if (string.IsNullOrEmpty(instanceName) == false)
                {
                    if (componentContext.TryResolveNamed(instanceName, parameterInfo.ParameterType, out var instance) == true)
                    {
                        // Return
                        return instance;
                    }
                }
            }

            // Reflection
            {
                // Instance
                var instance = Activator.CreateInstance(parameterInfo.ParameterType);

                // Bind
                instanceConfig.GetSection(parameterInfo.Name).Bind(instance);

                // Return
                return instance;
            }
        }
    }
}
