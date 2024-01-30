using MDP.Configuration;
using CLK.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Module;
using System;

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

            // ServiceCollection
            var serviceCollection = new ServiceCollection();
            {
                // Register
                serviceCollection.RegisterModule(configuration);
            }
            var serviceProvider = serviceCollection.BuildServiceProvider();
            if (serviceProvider == null) throw new InvalidOperationException($"{nameof(serviceProvider)}=null");

            // Resolve
            var messageContext = serviceProvider.ResolveTyped<MessageContext>();
            if (messageContext == null) throw new InvalidOperationException($"{nameof(messageContext)}=null");

            // Display
            Console.WriteLine(messageContext.GetValue());
        }
    }
}
