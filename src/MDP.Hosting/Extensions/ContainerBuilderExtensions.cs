using Autofac;
using Autofac.Builder;
using System;

namespace MDP
{
    public static class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterNamed<TService>(this ContainerBuilder container, Func<IComponentContext, string> setupAction) where TService : notnull
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (setupAction == null) throw new ArgumentException(nameof(setupAction));

            #endregion

            // Register
            var registrationBuilder = container.Register<TService>(componentContext =>
            {
                // ServiceName
                var serviceName = setupAction(componentContext);
                if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

                // Resolve
                return componentContext.ResolveNamed<TService>(serviceName);
            });

            // Return
            return registrationBuilder;
        }
    }
}