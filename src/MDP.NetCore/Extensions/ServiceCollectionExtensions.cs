﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MDP.NetCore;

namespace MDP
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection AddProgramService<TProgramService>(this IServiceCollection services) where TProgramService : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Add
            services.AddSingleton<IHostedService, ProgramService<TProgramService>>();

            // Return
            return services;
        }        

        public static IServiceCollection AddService<T>(this IServiceCollection services, Action<T> setupAction) where T : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));
            if (setupAction == null) throw new ArgumentException(nameof(setupAction));

            #endregion

            // Service
            var service = services.GetService<T>();
            if (service == null) throw new InvalidOperationException($"{typeof(T)} not found.");

            // Setup
            setupAction(service);

            // Return
            return services;
        }

        public static T GetService<T>(this IServiceCollection services) where T : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // GetService
            return (T)(services.LastOrDefault(service => service.ServiceType == typeof(T))?.ImplementationInstance);
        }
    }
}
