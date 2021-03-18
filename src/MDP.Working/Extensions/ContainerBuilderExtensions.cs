using Autofac;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Reflection;

namespace MDP
{
    public static class ContainerBuilderExtensions
    {
        // Methods
        public static void ScheduleJob<TJob>(this ContainerBuilder container, Action<TriggerBuilder> setupAction) where TJob : IJob
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (setupAction == null) throw new ArgumentException(nameof(setupAction));

            #endregion

            // ScheduleJob
            container.RegisterInstance<IConfigureOptions<QuartzOptions>>
            (
                new ConfigureNamedOptions<QuartzOptions>(Options.DefaultName, (options) =>
                {
                    var jobKey = JobKey.Create(nameof(TJob));
                    options.AddJob<TJob>(job => job.WithIdentity(jobKey));
                    options.AddTrigger(trigger =>
                    {
                        trigger.ForJob(jobKey);
                        trigger.WithIdentity($"{nameof(TJob)}-trigger-{Guid.NewGuid()}");
                        setupAction(trigger);
                    });
                })
            );
            container.RegisterType<TJob>();
        }
    }
}