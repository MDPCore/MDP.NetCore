using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Autofac
{
    public static partial class ComponentContextExtensions
    {
        // Methods
        public static TService ResolveRequired<TService>(this IComponentContext componentContext)
            where TService : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // ComponentContext
            if (typeof(TService) == typeof(IComponentContext)) return (TService)componentContext;

            // Service
            var service = componentContext.Resolve<TService>();
            if (service == null) throw new InvalidOperationException($"{nameof(service)}=null");

            // Return
            return service;
        }
    }

    public static partial class ComponentContextExtensions
    {
        // Methods
        internal static TService Build<TService>(this IComponentContext componentContext, Func<TService> serviceFactory)
            where TService : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Create
            return serviceFactory
            (
                
            );
        }

        internal static TService Build<TService, T1>(this IComponentContext componentContext, Func<T1, TService> serviceFactory)
            where TService : class
            where T1 : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Create
            return serviceFactory
            (
                componentContext.ResolveRequired<T1>()
            );
        }

        internal static TService Build<TService, T1, T2>(this IComponentContext componentContext, Func<T1, T2, TService> serviceFactory)
            where TService : class
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Create
            return serviceFactory
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>()
            );
        }

        internal static TService Build<TService, T1, T2, T3>(this IComponentContext componentContext, Func<T1, T2, T3, TService> serviceFactory)
            where TService : class
            where T1 : class
            where T2 : class
            where T3: class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Create
            return serviceFactory
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>(),
                componentContext.ResolveRequired<T3>()
            );
        }
    }
}