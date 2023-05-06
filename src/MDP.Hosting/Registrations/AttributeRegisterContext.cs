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
            var instanceTypeList = CLK.Reflection.Type.FindAllType();
            if (instanceTypeList == null) throw new InvalidOperationException($"{nameof(instanceTypeList)}=null");

            // InstanceType
            foreach (var instanceType in instanceTypeList)
            {
                // ServiceAttribute
                var serviceAttribute = instanceType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(ServiceAttribute<>)).OfType<ServiceAttribute>().FirstOrDefault();
                if (serviceAttribute == null) continue;

                // InstanceConfigList
                var instanceConfigList = new List<IConfigurationSection>();
                instanceConfigList.AddRange(this.FindAllServiceConfig(configuration, instanceType.Namespace!));
                instanceConfigList.AddRange(this.FindAllServiceConfig(configuration, serviceAttribute.ServiceType.Namespace!));

                // InstanceConfig
                foreach (var instanceConfig in instanceConfigList)
                {
                    // InstanceName
                    var instanceName = $"{instanceType.Namespace!}.{instanceConfig.Key}";
                    if(string.IsNullOrEmpty(instanceName)==true) throw new InvalidOperationException($"{nameof(instanceName)}=null");

                    // RegisterDefault
                    if (string.Equals(instanceConfig.Key, instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // Default
                        containerBuilder.Register((componentContext) =>
                        {
                            return componentContext.ResolveNamed(instanceName, serviceAttribute.ServiceType);
                        })
                        .As(serviceAttribute.ServiceType);
                    }

                    // RegisterNamed
                    if (instanceConfig.Key.StartsWith(instanceType.Name, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // ClassName
                        containerBuilder.Register((componentContext) =>
                        {
                            return componentContext.ResolveNamed(instanceName, serviceAttribute.ServiceType);
                        })
                        .Named(instanceConfig.Key, serviceAttribute.ServiceType);

                        // InstanceName
                        var registration = containerBuilder.Register((componentContext) =>
                        {
                            // Instance
                            var instance = this.CreateInstance(instanceType, instanceConfig, componentContext);
                            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                            // Return
                            return instance;
                        })
                        .Named(instanceName, serviceAttribute.ServiceType);
                        {
                            // Singleton
                            if (serviceAttribute.ServiceSingleton == true)
                            {
                                registration = registration.SingleInstance();
                            }
                        }
                    }
                }
            }
        }

        private object? CreateInstance(Type instanceType, IConfigurationSection instanceConfig, IComponentContext componentContext)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");

            #endregion

            // ConstructorInfo
            var constructorInfo = instanceType.GetConstructors().MaxBy(o=>o.GetParameters().Length);
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
        }

        private object? CreateParameter(ParameterInfo parameterInfo, IConfigurationSection instanceConfig, IComponentContext componentContext)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");

            #endregion

            // ParameterConfig
            var parameterConfig = instanceConfig.GetSection(parameterInfo.Name);
            if (parameterInfo.HasDefaultValue == true && parameterConfig == null) return parameterInfo.DefaultValue;
            if (parameterInfo.HasDefaultValue == true && parameterConfig.Exists() == false) return parameterInfo.DefaultValue;

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
                if (string.IsNullOrEmpty(instanceName) == true)
                {
                    if (componentContext.TryResolve(parameterInfo.ParameterType, out var instance) == true)
                    {
                        // Return
                        return instance;
                    }
                }
                else
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
