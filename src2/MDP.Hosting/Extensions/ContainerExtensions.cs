using Autofac;

namespace MDP.Hosting
{
    public static class ContainerExtensions
    {
        // Methods
        public static TService Resolve<TService>(this IComponentContext componentContext, string serviceName) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (string.IsNullOrEmpty(serviceName) == true) throw new ArgumentException(nameof(serviceName));

            #endregion

            // Resolve
            return componentContext.ResolveNamed<TService>(serviceName);
        }

        public static List<TService> ResolveAll<TService>(this IComponentContext componentContext) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Return
            return componentContext.Resolve<IList<TService>>().ToList();
        }

        public static List<TService> ResolveAll<TService>(this IComponentContext componentContext, List<string> serviceNameList) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceNameList == null) throw new ArgumentException(nameof(serviceNameList));

            #endregion

            // Result
            var serviceList = new List<TService>();

            // ServiceNameList
            foreach (var sreviceName in serviceNameList)
            {
                // Resolve
                var service = componentContext.ResolveNamed<TService>(sreviceName);
                if (service == null) throw new InvalidOperationException($"{nameof(service)}=null");

                // Add
                serviceList.Add(service);
            }

            // Return
            return serviceList;
        }
    }
}
