using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    public static partial class ServiceCollectionExtensions
    {
        // Methods
        public static void RegisterTyped(this IServiceCollection serviceCollection, Type serviceType, Func<IServiceProvider, object> resolveAction, bool singleton = false)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // Register
            if (singleton == true)
            {
                // AddSingleton
                serviceCollection.AddSingleton(serviceType, resolveAction);
            }
            else
            {
                // AddTransient
                serviceCollection.AddTransient(serviceType, resolveAction);
            }
        }

        public static void RegisterTyped<TService>(this IServiceCollection serviceCollection, Func<IServiceProvider, object> resolveAction, bool singleton = false) where TService : class
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // RegisterTyped
            serviceCollection.RegisterTyped(typeof(TService), resolveAction, singleton);
        }


        public static void RegisterNamed(this IServiceCollection serviceCollection, Type serviceType, string instanceName, Func<IServiceProvider, object> resolveAction, bool singleton = false)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // ServiceBuilderType
            var serviceBuilderType = typeof(ServiceBuilder<>).MakeGenericType(serviceType);
            if (serviceBuilderType == null) throw new InvalidOperationException($"{nameof(serviceBuilderType)}=null");

            // RegisterTyped
            serviceCollection.RegisterTyped(serviceBuilderType, (serviceProvider) =>
            {
                return ServiceActivator.CreateInstance(serviceBuilderType, new List<object>() { instanceName, resolveAction });
            }, singleton);
        }

        public static void RegisterNamed<TService>(this IServiceCollection serviceCollection, string instanceName, Func<IServiceProvider, object> resolveAction, bool singleton = false) where TService : class
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // RegisterNamed
            serviceCollection.RegisterNamed(typeof(TService), instanceName, resolveAction, singleton);
        }
    }

    public static partial class ServiceCollectionExtensions
    {
        // Methods
        public static void RegisterModule(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // DefaultRegister
            {
                // List
                serviceCollection.TryAddTransient(typeof(IList<>), typeof(List<>));
            }

            // ServiceRegister
            ServiceRegister.RegisterModule(serviceCollection, configuration);

            // FactoryRegister
            FactoryRegister.RegisterModule(serviceCollection, configuration);
                
            // ServiceRegistrationRegister
            ServiceRegistrationRegister.RegisterModule(serviceCollection, typeof(IServiceCollection));
        }
    }
}