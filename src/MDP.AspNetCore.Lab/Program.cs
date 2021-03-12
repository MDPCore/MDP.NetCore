using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Lab
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
                .ConfigureMDP()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
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
