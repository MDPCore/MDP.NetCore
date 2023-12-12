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
        public static object CreateInstance(Type instanceType, ParameterProvider parameterProvider, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException($"{nameof(instanceType)}=null");
            if (parameterProvider == null) throw new ArgumentException($"{nameof(parameterProvider)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ConstructorInfo
            var constructorInfo = instanceType.GetConstructors().MaxBy(o => o.GetParameters().Length);
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // ParameterList
            var parameterList = new List<object>();
            foreach (var parameterInfo in constructorInfo.GetParameters())
            {
                parameterList.Add(ParameterActivator.CreateParameter(parameterInfo, parameterProvider, serviceProvider));
            }

            // Instance
            var instance = constructorInfo.Invoke(parameterList.ToArray());
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }

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
    }
}
