using MDP.NetCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;

namespace MDP.Quartz
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static void ScheduleJob<TJob>(this IServiceCollection services, Action<TriggerBuilder> setupAction) where TJob : class, IJob
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));
            if (setupAction == null) throw new ArgumentException(nameof(setupAction));

            #endregion

            // ScheduleJob
            services.Configure<QuartzOptions>((options) =>
            {
                var jobKey = JobKey.Create(nameof(TJob));
                options.AddJob<TJob>(job => job.WithIdentity(jobKey));
                options.AddTrigger(trigger =>
                {
                    trigger.ForJob(jobKey);
                    trigger.WithIdentity($"{nameof(TJob)}-trigger");
                    setupAction(trigger);
                });
            });
            services.AddTransient<TJob>();
        }
    }
}
