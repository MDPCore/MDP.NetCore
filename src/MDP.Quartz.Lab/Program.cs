using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MDP.NetCore;
using Quartz;

namespace MDP.Quartz.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureMDP(mdpBuilder=> 
                {
                    // Quartz
                    mdpBuilder.AddQuartz();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // SettingContextJob
                    services.ScheduleJob<SettingContextJob>((trigger) =>
                    {
                        // Trigger
                        trigger.WithCronSchedule("0/1 * * * * ?");
                    });
                });


        // Class
        public class SettingContext
        {
            // Methods
            public string GetValue()
            {
                // Return
                return "Hello World!";
            }
        }

        public class SettingContextModule : MDP.Hosting.Module
        {
            // Methods
            protected override void ConfigureContainer(ContainerBuilder container)
            {
                #region Contracts

                if (container == null) throw new ArgumentException(nameof(container));

                #endregion

                // SettingContext
                {
                    // Register
                    container.RegisterType<SettingContext>().As<SettingContext>();
                }
            }
        }

        [DisallowConcurrentExecution]
        public class SettingContextJob : IJob
        {
            // Fields
            private readonly SettingContext _settingContext = null;


            // Constructors
            public SettingContextJob(SettingContext settingContext)
            {
                #region Contracts

                if (settingContext == null) throw new ArgumentException(nameof(settingContext));

                #endregion

                // Default
                _settingContext = settingContext;
            }


            // Methods
            public Task Execute(IJobExecutionContext context)
            {
                #region Contracts

                if (context == null) throw new ArgumentException(nameof(context));

                #endregion

                // Execute
                return Task.Run(() =>
                {
                    // SettingContext
                    Console.WriteLine(_settingContext.GetValue());
                });
            }
        }
    }
}