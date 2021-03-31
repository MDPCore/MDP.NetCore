using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace MDP
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods 
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<TService>(this ContainerBuilder container)
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // RegisterInterface
            return container.RegisterInterface<IConfiguration, TService>(configuration =>
            {
                // ServiceName
                var serviceName = configuration.GetServiceType<TService>();
                if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

                // Return
                return serviceName;
            });
        }

        public static IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterImplementation<TService, TImplementation>(this ContainerBuilder container)
            where TImplementation : TService
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // ServiceName
            var serviceName = typeof(TImplementation).FullName;
            if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

            // Return
            return container.RegisterType<TImplementation>().Named<TService>(serviceName);
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods  
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<TService>(this ContainerBuilder container, Func<string> nameAction)
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (nameAction == null) throw new ArgumentException(nameof(nameAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(nameAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<T1, TService>(this ContainerBuilder container, Func<T1, string> nameAction)
            where T1 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (nameAction == null) throw new ArgumentException(nameof(nameAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(nameAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, string> nameAction)
            where T1 : class
            where T2 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (nameAction == null) throw new ArgumentException(nameof(nameAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(nameAction));
            });

            // Return
            return registrationBuilder;
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, string> nameAction)
            where T1 : class
            where T2 : class
            where T3 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (nameAction == null) throw new ArgumentException(nameof(nameAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.ResolveNamed<TService>(() => componentContext.Build(nameAction));
            });

            // Return
            return registrationBuilder;
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods   
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<TService>(this ContainerBuilder container, Func<TService> buildAction) 
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(buildAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, TService>(this ContainerBuilder container, Func<T1, TService> buildAction)
            where T1 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(buildAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, TService> buildAction)
            where T1 : class
            where T2 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(buildAction);
            });
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, TService> buildAction)
            where T1 : class
            where T2 : class
            where T3 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (buildAction == null) throw new ArgumentException(nameof(buildAction));

            #endregion

            // Register
            return container.Register<TService>(componentContext =>
            {
                // Resolve
                return componentContext.Build(buildAction);
            });
        }
    }
}