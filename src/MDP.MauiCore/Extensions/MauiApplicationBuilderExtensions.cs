using MDP.Configuration;
using MDP.Hosting;
using MDP.NetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;

namespace MDP.MauiCore
{
    internal static class MauiApplicationBuilderExtensions
    {
        // Methods
        public static MauiAppBuilder ConfigureMDP<TApp>(this MauiAppBuilder applicationBuilder) where TApp : class, IApplication
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");

            #endregion

            // EntryAssembly
            var entryAssembly = typeof(TApp).Assembly;
            if (entryAssembly == null) throw new InvalidDataException($"{nameof(entryAssembly)}=null");

            // EnvironmentVariables
            var environmentVariables = new MauiEnvironmentVariables();
            if(environmentVariables==null ) throw new InvalidDataException($"{nameof(environmentVariables)}=null");

            // HostEnvironment
            var hostEnvironment = new MauiHostEnvironment(environmentVariables, entryAssembly);
            if (environmentVariables == null) throw new InvalidDataException($"{nameof(environmentVariables)}=null");

            // ConfigurationBuilder
            var configurationBuilder = applicationBuilder.Configuration;
            {
                // ConfigurationRegister
                ConfigurationRegister.RegisterModule(configurationBuilder, new MDP.Configuration.EmbeddedConfigurationProvider(entryAssembly, hostEnvironment.EnvironmentName));
            }

            // ContainerBuilder
            var serviceCollection = applicationBuilder.Services;
            {
                // ContainerRegister
                {
                    ServiceFactoryRegister.RegisterModule(applicationBuilder, applicationBuilder.Configuration);
                }
                ContainerRegister.RegisterModule(serviceCollection, applicationBuilder.Configuration);

                // EnvironmentVariables
                serviceCollection.AddSingleton<IEnvironmentVariables>(environmentVariables);

                // HostEnvironment
                serviceCollection.AddSingleton<IHostEnvironment>(hostEnvironment);
            }

            // MauiBuilder
            {
                // Debug
                if (hostEnvironment.IsDevelopment() == true)
                {
                    applicationBuilder.Services.AddBlazorWebViewDeveloperTools();
                    applicationBuilder.Logging.AddDebug();
                }

                // MauiApp
                applicationBuilder.UseMauiApp<TApp>();

                // Font
                applicationBuilder.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

                // Blazor
                applicationBuilder.Services.AddMauiBlazorWebView();
            }

            // Return
            return applicationBuilder;
        }
    }
}
