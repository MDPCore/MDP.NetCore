using MDP.Configuration;
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

            // ConfigurationBuilder
            var configurationBuilder = new ConfigurationBuilder();
            {
                // Register
                configurationBuilder.RegisterModule(environmentName);
            }
            var configuration = configurationBuilder.Build();
            if (configuration == null) throw new InvalidOperationException($"{nameof(configuration)}=null");

            // ContainerBuilder
            var containerBuilder = new ServiceCollection();
            {
                // Register
                containerBuilder.RegisterModule(configuration);
            }
            var container = containerBuilder.BuildServiceProvider();
            if (container == null) throw new InvalidOperationException($"{nameof(container)}=null");

            // Resolve
            var messageContext = container.ResolveTyped<MessageContext>();
            if (messageContext == null) throw new InvalidOperationException($"{nameof(messageContext)}=null");

            // Display
            Console.WriteLine(messageContext.GetValue());
        }
    }
}
