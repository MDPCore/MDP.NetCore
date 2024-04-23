using MDP.Configuration;
using MDP.Hosting;
using MDP.NetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System.Collections.Generic;
using System.Reflection;

namespace MDP.BlazorCore.Maui
{
    internal static class MauiApplicationBuilderExtensions
    {
        // Methods
        public static MauiAppBuilder ConfigureMDP<TProgram>(this MauiAppBuilder applicationBuilder, Type defaultLayout = null) where TProgram : class
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");

            #endregion

            // EntryAssembly
            var entryAssembly = typeof(TProgram).Assembly;
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");

            // HostEnvironment
            var hostEnvironment = new MauiHostEnvironment(new MauiEnvironmentVariables(), entryAssembly);
            if (hostEnvironment == null) throw new InvalidOperationException($"{nameof(hostEnvironment)}=null");

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

                // HostEnvironment
                serviceCollection.AddSingleton<IHostEnvironment>(hostEnvironment);
            }

            // BlazorBuilder
            {
                // MauiApp
                applicationBuilder.UseMauiApp<MDP.BlazorCore.Maui.App>();
                applicationBuilder.Services.AddMauiBlazorWebView();
                applicationBuilder.Services.AddAuthorizationCore();

                // BlazorApp
                applicationBuilder.Services.AddSingleton<RoutesOptions>(serviceProvider => { 
                    return new RoutesOptions()
                    {
                        AppAssembly = entryAssembly,
                        DefaultLayout = defaultLayout
                    };
                });

                // BlazorTools
                if (hostEnvironment.IsDevelopment() == true)
                {
                    applicationBuilder.Services.AddBlazorWebViewDeveloperTools();
                    applicationBuilder.Logging.AddDebug();
                }
            }

            // Return
            return applicationBuilder;
        }
    }
}
