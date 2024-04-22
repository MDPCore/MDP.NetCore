using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    internal static class ServiceRegister
    {
        // Methods
        public static void RegisterService(IServiceCollection serviceCollection, Type serviceType, Type instanceType, string instanceName, MDP.Reflection.ParameterProvider parameterProvider, bool singleton)
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
                // ServiceParameterProvider
                var serviceParameterProvider = new ServiceParameterProvider(serviceProvider, parameterProvider);

                // ConstructorInfo
                var constructorInfo = instanceType.GetConstructors().MaxBy(o => o.GetParameters().Length);
                if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

                // Parameters
                var parameters = new List<object>();
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    parameters.Add(serviceParameterProvider.GetValue(parameterInfo.ParameterType, parameterInfo.Name, parameterInfo.HasDefaultValue, parameterInfo.DefaultValue));
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
}
