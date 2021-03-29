using Autofac;
using Autofac.Builder;
using System;

namespace MDP
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods     
        public static IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterNamed<TImplementer, TService>(this ContainerBuilder container, string serviceName = null)
            where TImplementer : notnull
            where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // ServiceName
            serviceName = serviceName ?? typeof(TImplementer).Name;
            if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

            // Return
            return container.RegisterType<TImplementer>().Named<TService>(serviceName);
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods  
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterSelected<TService>(this ContainerBuilder container, Func<string> selectAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (selectAction == null) throw new ArgumentException(nameof(selectAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(selectAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterSelected<T1, TService>(this ContainerBuilder container, Func<T1, string> selectAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (selectAction == null) throw new ArgumentException(nameof(selectAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(selectAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterSelected<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, string> selectAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (selectAction == null) throw new ArgumentException(nameof(selectAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(selectAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterSelected<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, string> selectAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (selectAction == null) throw new ArgumentException(nameof(selectAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(selectAction));
            });

            // Return
            return registrationBuilder;
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods   
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<TService>(this ContainerBuilder container, Func<TService> resolveAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(resolveAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, TService>(this ContainerBuilder container, Func<T1, TService> resolveAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(resolveAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, TService> resolveAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(resolveAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, TService> resolveAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(resolveAction);
            });
        }
    }
}