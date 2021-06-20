using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection RemoveService<TService>(this IServiceCollection services) where TService : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // ServiceDescriptor
            var serviceDescriptor = services.FirstOrDefault(o => o.ImplementationType == typeof(TService));
            if (serviceDescriptor == null) return services;

            // Remove
            services.Remove(serviceDescriptor);

            // Return
            return services;
        }
    }
}
