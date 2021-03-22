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
        public static void Run(SettingContext settingContext)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // SettingContext
            Console.WriteLine(settingContext.GetValue());
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureMDP(mdpBuilder =>
                {
                    // Nothing

                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Program
                    services.AddProgram<Program>();
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

        public class SettingContextModule : MDP.Module
        {
            // Methods
            protected override void ConfigureContainer(ContainerBuilder container)
            {
                #region Contracts

                if (container == null) throw new ArgumentException(nameof(container));

                #endregion

                // Register
                container.RegisterType<SettingContext>().As<SettingContext>();
            }
        }
    }
}