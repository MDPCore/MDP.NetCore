using CLK.ComponentModel;
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
    }

    public static partial class ServiceCollectionExtensions
    {
        // Methods
        public static void RegisterNamed(this IServiceCollection serviceCollection, Type serviceType, string instanceName, Func<IServiceProvider, object> resolveAction, bool singleton = false)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (serviceType == null) throw new ArgumentException($"{nameof(serviceType)}=null");
            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // NamedServiceBuilder
            var namedServiceBuilderType = typeof(NamedServiceBuilder<>).MakeGenericType(serviceType);
            if (namedServiceBuilderType == null) throw new InvalidOperationException($"{nameof(namedServiceBuilderType)}=null");

            // RegisterTyped
            serviceCollection.RegisterTyped(namedServiceBuilderType, (serviceProvider) =>
            {
                // NamedServiceBuilder
                var namedServiceBuilder = Activator.CreateInstance(namedServiceBuilderType, new object[] { instanceName, resolveAction });
                if (namedServiceBuilder == null) throw new InvalidOperationException($"{nameof(namedServiceBuilder)}=null");

                // Return
                return namedServiceBuilder;
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

            // ServiceFactoryRegister
            ServiceFactoryRegister.RegisterModule(serviceCollection, configuration);
            ServiceFactoryRegister.RegisterModule(new ServiceCollectionBuilder(serviceCollection), configuration);

            // ServiceAttributeRegister
            ServiceAttributeRegister.RegisterModule(serviceCollection, configuration);            

            // ServiceRegistrationRegister
            ServiceRegistrationRegister.RegisterModule(serviceCollection);
        }


        // Class
        private class ServiceCollectionBuilder : ServiceBuilder
        {
            // Fields
            private readonly IServiceCollection _serviceCollection = null;


            // Constructors
            public ServiceCollectionBuilder(IServiceCollection serviceCollection)
            {
                #region Contracts

                if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

                #endregion

                // Default
                _serviceCollection = serviceCollection;
            }


            // Methods
            public void Add(ServiceRegistration serviceRegistration)
            {
                #region Contracts

                if (serviceRegistration == null) throw new ArgumentException($"{nameof(serviceRegistration)}=null");

                #endregion

                // Register
                _serviceCollection.AddSingleton(serviceRegistration);
            }
        }
    }
}