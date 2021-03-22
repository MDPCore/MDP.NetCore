using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.NetCore;
using Microsoft.Extensions.Options;

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Startup
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureMDP(mdpBuilder =>
                {
                    // Mvc
                    mdpBuilder.AddMvc();
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
