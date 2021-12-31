using Autofac;
using Microsoft.Extensions.Configuration;
using System;

namespace MDP.Hosting.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // ConfigurationBuilder
            var configurationBuilder = new ConfigurationBuilder();
            {
                configurationBuilder.SetBasePath(CLK.IO.Directory.GetEntryDirectory());
                configurationBuilder.AddJsonFile("appsettings.json");
                configurationBuilder.RegisterModule();
            }

            // ContainerBuilder
            var containerBuilder = new ContainerBuilder();
            {
                // Register
                containerBuilder.RegisterInstance(configurationBuilder.Build()).As<IConfiguration>();
                containerBuilder.RegisterModule();
            }

            // Container
            using (var container = containerBuilder.Build())
            {
                // Resolve
                var workService = container.Resolve<WorkService>();
                if (workService == null) throw new InvalidOperationException($"{nameof(workService)}=null");

                // ResolveAAA
                var workServiceAAA = container.Resolve<WorkService>("AAA");
                if (workServiceAAA == null) throw new InvalidOperationException($"{nameof(workServiceAAA)}=null");

                // Execute
                workService.Execute();
                workServiceAAA.Execute();
            }
        }
    }
}
