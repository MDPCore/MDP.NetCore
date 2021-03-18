using Autofac;
using Microsoft.Extensions.Configuration;
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
        public static void Run(SettingContext settingContext, IConfiguration configuration)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // SettingContext
            Console.WriteLine(settingContext.GetValue());

            // Configuration
            Console.WriteLine(configuration.GetValue<string>("Setting01"));
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
                    // Program
                    services.AddProgram<Program>();
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
            public string GetValue()
            {
                // Return
                return "Hello World!";
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