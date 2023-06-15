using System;
using System.Collections.Generic;

namespace MDP.NetCore
{
    public static class ServiceProviderExtensions
    {
        // Methods
        public static TService ResolveTyped<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ResolveTyped
            var instance = serviceProvider.ResolveTyped(typeof(TService)) as TService;
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }

        public static TService ResolveNamed<TService>(this IServiceProvider serviceProvider, string instanceName)
            where TService : class
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");

            #endregion

            // ResolveNamed
            var instance =  serviceProvider.ResolveNamed(typeof(TService), instanceName) as TService;
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }


        public static object ResolveTyped(this IServiceProvider serviceProvider, Type serviceType)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");

            #endregion

            // Result
            object? instance = null;

            // ResolveTyped
            serviceProvider.TryResolveTyped(serviceType, out instance);
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }

        public static object ResolveNamed(this IServiceProvider serviceProvider, Type serviceType, string instanceName)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");

            #endregion

            // Result
            object? instance = null;

            // ResolveNamed
            serviceProvider.TryResolveNamed(serviceType, instanceName, out instance);
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Return
            return instance;
        }


        public static bool TryResolveTyped(this IServiceProvider serviceProvider, Type serviceType, out object? instance)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");

            #endregion

            // Result
            instance = null;

            // Resolve
            instance = serviceProvider.GetService(serviceType);
            if(instance==null) return false;

            // Return
            return true;
        }

        public static bool TryResolveNamed(this IServiceProvider serviceProvider, Type serviceType, string instanceName, out object? instance)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");

            #endregion

            // Result
            instance = null;

            // ServiceRegisterList
            var serviceRegisterList = serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(typeof(ServiceRegister<>).MakeGenericType(serviceType))) as IEnumerable<ServiceRegister>;
            if (serviceRegisterList == null) throw new InvalidOperationException($"{nameof(serviceRegisterList)}=null");

            // Resolve
            foreach (var serviceRegister in serviceRegisterList)
            {
                if (string.Equals(serviceRegister.InstanceName, instanceName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Resolve
                    instance = serviceRegister.Resolve(serviceProvider);
                    if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                    // Return
                    return true;
                }
            }

            // Return
            return false;
        }
    }
}
