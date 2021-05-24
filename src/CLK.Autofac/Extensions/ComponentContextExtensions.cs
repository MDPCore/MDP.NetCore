using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Autofac
{
    internal static partial class ComponentContextExtensions
    {
        // Methods
        public static TResult Build<TResult>(this IComponentContext componentContext, Func<TResult> buildAction)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Build
            return buildAction
            (
                
            );
        }

        public static TResult Build<T1, TResult>(this IComponentContext componentContext, Func<T1, TResult> buildAction)
            where T1 : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Build
            return buildAction
            (
                componentContext.ResolveRequired<T1>()
            );
        }

        public static TResult Build<T1, T2, TResult>(this IComponentContext componentContext, Func<T1, T2, TResult> buildAction)
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Build
            return buildAction
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>()
            );
        }

        public static TResult Build<T1, T2, T3, TResult>(this IComponentContext componentContext, Func<T1, T2, T3, TResult> buildAction)
            where T1 : class
            where T2 : class
            where T3: class
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Build
            return buildAction
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>(),
                componentContext.ResolveRequired<T3>()
            );
        }
    }

    internal static partial class ComponentContextExtensions
    {
        // Methods
        private static TService ResolveRequired<TService>(this IComponentContext componentContext) 
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
}