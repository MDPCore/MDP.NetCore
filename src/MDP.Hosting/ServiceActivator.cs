using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDP.Hosting
{
    internal class ServiceActivator
    {
        // Methods
        public static object CreateInstance(Type instanceType, List<object> parameterList)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (parameterList == null) throw new ArgumentException($"{nameof(parameterList)}=null");

            #endregion

            // ConstructorInfo
            var constructorInfo = instanceType.GetConstructors().FirstOrDefault(o => o.GetParameters().Length == parameterList.Count);
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // Instance
            var instance = constructorInfo.Invoke(parameterList.ToArray());
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }

        public static object CreateInstance(Type instanceType, IConfigurationSection instanceConfig, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ConstructorInfo
            var constructorInfo = instanceType.GetConstructors().MaxBy(o => o.GetParameters().Length);
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // ParameterList
            var parameterList = new List<object>();
            foreach (var parameterInfo in constructorInfo.GetParameters())
            {
                parameterList.Add(ServiceActivator.CreateParameter(parameterInfo, instanceConfig, serviceProvider));
            }

            // Instance
            var instance = constructorInfo.Invoke(parameterList.ToArray());
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }

        private static object CreateParameter(ParameterInfo parameterInfo, IConfigurationSection instanceConfig, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (instanceConfig == null) throw new ArgumentException($"{nameof(instanceConfig)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

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
                // ParameterName
                var parameterName = instanceConfig.GetValue<string>(parameterInfo.Name);
                if (string.IsNullOrEmpty(parameterName) == true)
                {
                    // ResolveTyped
                    if (serviceProvider.TryResolveTyped(parameterInfo.ParameterType, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
                else
                {
                    // ResolveNamed
                    if (serviceProvider.TryResolveNamed(parameterInfo.ParameterType, parameterName, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
            }

            // Reflection
            {
                // Create
                var parameter = Activator.CreateInstance(parameterInfo.ParameterType);
                if (parameter == null) throw new InvalidOperationException($"{nameof(parameter)}=null");

                // Bind
                instanceConfig.GetSection(parameterInfo.Name).Bind(parameter);

                // Return
                return parameter;
            }
        }
    }
}
