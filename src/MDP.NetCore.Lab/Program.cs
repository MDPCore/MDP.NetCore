using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(SettingContext settingContext)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // Execute
            settingContext.Execute();
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureMDP()
                .ConfigureServices((hostContext, services) =>
                {
                    // ConsoleService
                    services.AddConsoleService<Program>();
                });


        // Class
        public class SettingContext
        {
            // Constructors
            public SettingContext()
            {
                // Display
                Console.WriteLine("SettingContext Created");
            }


            // Methods
            public void Execute()
            {
                // Execute
                Console.WriteLine("Hello World!");
            }
        }

        public class SettingContextModule : MDP.Module
        {
            // Methods
            protected override void Load(ContainerBuilder builder)
            {
                #region Contracts

                if (builder == null) throw new ArgumentException(nameof(builder));

                #endregion

                // SettingContext
                {
                    // Register
                    builder.RegisterType<SettingContext>().As<SettingContext>();
                }
            }
        }
    }
}