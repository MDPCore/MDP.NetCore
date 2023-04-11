using Autofac;
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
    internal class AttributeRegisterContext : RegisterContext, IDisposable
    {
        // Constructors
        public AttributeRegisterContext()
        {

        }

        public void Dispose()
        {
            
        }


        // Methods
        public void RegisterModule(ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // InstanceTypeList
            var instanceTypeList = this.FindAllModuleType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // InstanceType
            foreach (var instanceType in instanceTypeList)
            {
                // ServiceAttribute
                var serviceAttribute = instanceType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(ServiceAttribute<>)).OfType<ServiceAttribute>().FirstOrDefault();
                if (serviceAttribute == null) continue;

                // InstanceConfigList
                var instanceConfigList = this.FindAllServiceConfig(configuration, serviceAttribute.ServiceNamespace);
                if (instanceConfigList == null) throw new InvalidOperationException($"{nameof(instanceConfigList)}=null");

                // InstanceConfig
                foreach (var instanceConfig in instanceConfigList)
                {
                    // DefaultInstanceConfig
                    if (string.Equals(instanceConfig.Key, instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // RegisterDefault
                        containerBuilder.Register((componentContext) =>
                        {
                            return componentContext.ResolveNamed(instanceConfig.Key, serviceAttribute.ServiceType);
                        })
                        .As(serviceAttribute.ServiceType);

                        // RegisterService
                        this.RegisterService(containerBuilder, instanceType, instanceConfig, serviceAttribute);

                        // Continue
                        continue;
                    }

                    // NamedInstanceConfig
                    if (instanceConfig.Key.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // RegisterService
                        this.RegisterService(containerBuilder, instanceType, instanceConfig, serviceAttribute);

                        // Continue
                        continue;
                    }
                }
            }
        }

        private void RegisterService(ContainerBuilder containerBuilder, Type instanceType, IConfigurationSection instanceConfig, ServiceAttribute serviceAttribute)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (serviceAttribute == null) throw new ArgumentException($"{nameof(serviceAttribute)}=null");

            #endregion

            // Register
            var registration = containerBuilder.Register((componentContext) =>
            {
                // ConstructorInfo
                var constructorInfo = instanceType.GetConstructors().FirstOrDefault();
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
            registration = registration.Named(instanceConfig.Key, serviceAttribute.ServiceType);

            // Singleton
            if (serviceAttribute.ServiceSingleton == true)
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
                if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                // Bind
                instanceConfig.GetSection(parameterInfo.Name).Bind(instance);

                // Return
                return instance;
            }
        }
    }
}
