using MDP.Configuration;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Module;
using System;
using System.Diagnostics;

namespace MDP.Hosting.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // EnvironmentName
            var environmentName = "Production";
            //var environmentName = "Staging";
            //var environmentName = "Development";

            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            {
                // Register
                configurationBuilder.RegisterModule(environmentName);
            }
            var configuration = configurationBuilder.Build();
            if (configuration == null) throw new InvalidOperationException($"{nameof(configuration)}=null");

            // ServiceProvider
            var serviceCollection = new ServiceCollection();
            {
                // Register
                serviceCollection.RegisterModule(configuration);
            }
            var serviceProvider = serviceCollection.BuildServiceProvider();
            if (serviceProvider == null) throw new InvalidOperationException($"{nameof(serviceProvider)}=null");

            // Resolve
            var workContext = serviceProvider.ResolveTyped<WorkContext>();
            if (workContext == null) throw new InvalidOperationException($"{nameof(workContext)}=null");
            
            // Display
            Console.WriteLine(workContext.GetValue());
        }
    }
}
