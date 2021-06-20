using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public static class SecurityTokenFactoryExtensions
    {
        // Methods
        public static IServiceCollection AddSecurityTokenFactory(this IServiceCollection services, Action<SecurityTokenFactoryOptions> configureOptions = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Options
            if (configureOptions != null) services.Configure(configureOptions);

            // Service
            services.TryAddSingleton<SecurityTokenFactory>();

            // Return
            return services;
        }
    }
}
