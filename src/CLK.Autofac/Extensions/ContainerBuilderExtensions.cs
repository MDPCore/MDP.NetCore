using Autofac;
using Autofac.Builder;
using System;

namespace CLK.Autofac
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods   
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<TService>(this ContainerBuilder container, Func<TService> serviceFactory)
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

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, TService>(this ContainerBuilder container, Func<T1, TService> serviceFactory)
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

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, TService> serviceFactory)
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

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> Register<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, TService> serviceFactory)
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

    public static partial class ContainerBuilderExtensions
    {
        // Methods 
        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<TService>(this ContainerBuilder container, Func<ParameterDictionary> parametersFactory)
             where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (parametersFactory == null) throw new ArgumentException(nameof(parametersFactory));

            #endregion

            // RegisterType
            return container.RegisterType<TService>().WithParameter
            (
                // ParameterSelector
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return false;

                    // Return
                    return true;
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return null;

                    // Return
                    return valueObject;
                }
            );
        }

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<T1, TService>(this ContainerBuilder container, Func<T1, ParameterDictionary> parametersFactory)
            where T1 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (parametersFactory == null) throw new ArgumentException(nameof(parametersFactory));

            #endregion

            // RegisterType
            return container.RegisterType<TService>().WithParameter
            (
                // ParameterSelector
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return false;

                    // Return
                    return true;
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return null;

                    // Return
                    return valueObject;
                }
            );
        }

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<T1, T2, TService>(this ContainerBuilder container, Func<T1, T2, ParameterDictionary> parametersFactory)
            where T1 : class
            where T2 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (parametersFactory == null) throw new ArgumentException(nameof(parametersFactory));

            #endregion

            // RegisterType
            return container.RegisterType<TService>().WithParameter
            (
                // ParameterSelector
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return false;

                    // Return
                    return true;
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return null;

                    // Return
                    return valueObject;
                }
            );
        }

        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<T1, T2, T3, TService>(this ContainerBuilder container, Func<T1, T2, T3, ParameterDictionary> parametersFactory)
            where T1 : class
            where T2 : class
            where T3 : class
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (parametersFactory == null) throw new ArgumentException(nameof(parametersFactory));

            #endregion

            // RegisterType
            return container.RegisterType<TService>().WithParameter
            (
                // ParameterSelector
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return false;

                    // Return
                    return true;
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    // Build
                    var parameters = componentContext.Build(parametersFactory);
                    if (parameters == null) throw new InvalidOperationException($"{nameof(parameters)}=null");

                    // ValueObject
                    var valueObject = parameters.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (valueObject == null) return null;

                    // Return
                    return valueObject;
                }
            );
        }
    }
}