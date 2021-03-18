using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;

namespace MDP.Working
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Quartz
            services.AddQuartz(options =>
            {
                // ScopedJob
                options.UseMicrosoftDependencyInjectionScopedJobFactory();
            });
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            // Return
            return services;
        }
    }
}
