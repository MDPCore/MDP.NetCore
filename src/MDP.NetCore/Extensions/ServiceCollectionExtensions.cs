using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MDP.NetCore;

namespace MDP.NetCore
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection AddProgram<TProgram>(this IServiceCollection services) where TProgram : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Add
            services.AddSingleton<IHostedService, ProgramService<TProgram>>();

            // Return
            return services;
        }        
    }
}
