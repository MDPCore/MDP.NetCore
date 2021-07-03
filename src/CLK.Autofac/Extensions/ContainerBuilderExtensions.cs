using Autofac;
using Autofac.Builder;
using System;

namespace CLK.Autofac
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods   
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterService<TService>(this ContainerBuilder container, Func<TService> serviceFactory)
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Build
                return componentContext.Build(serviceFactory);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterService<TService, T1>(this ContainerBuilder container, Func<T1, TService> serviceFactory)
            where T1 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Build
                return componentContext.Build(serviceFactory);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterService<TService, T1, T2>(this ContainerBuilder container, Func<T1, T2, TService> serviceFactory)
            where T1 : class
            where T2 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Build
                return componentContext.Build(serviceFactory);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterService<TService, T1, T2, T3>(this ContainerBuilder container, Func<T1, T2, T3, TService> serviceFactory)
            where T1 : class
            where T2 : class
            where T3 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (serviceFactory == null) throw new ArgumentException(nameof(serviceFactory));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Build
                return componentContext.Build(serviceFactory);
            });
        }
    }
}