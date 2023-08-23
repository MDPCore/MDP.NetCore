﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static void RegisterTyped(this IServiceCollection serviceCollection, Type serviceType, Func<IServiceProvider, object> resolveAction, bool singleton)
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

        public static void RegisterNamed(this IServiceCollection serviceCollection, Type serviceType, string instanceName, Func<IServiceProvider, object> resolveAction, bool singleton)
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
    }
}