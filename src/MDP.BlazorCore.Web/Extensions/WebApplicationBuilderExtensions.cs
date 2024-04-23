using MDP.AspNetCore;
using MDP.Configuration;
using MDP.Hosting;
using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MDP.BlazorCore.Web
{
    internal static class WebApplicationBuilderExtensions
    {
        // Methods
        public static WebApplicationBuilder ConfigureMDP<TApp>(this WebApplicationBuilder applicationBuilder, Type defaultLayout = null) where TApp : ComponentBase
        {
            #region Contracts

            if (applicationBuilder == null) throw new ArgumentException($"{nameof(applicationBuilder)}=null");

            #endregion

            // Base
            applicationBuilder = MDP.AspNetCore.WebApplicationBuilderExtensions.ConfigureMDP(applicationBuilder);

            // EntryAssembly
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");

            // BlazorBuilder
            var componentsBuilder = applicationBuilder.Services.AddRazorComponents();
            {
                // Rendermode
                componentsBuilder.AddInteractiveServerComponents();

                // BlazorApp
                applicationBuilder.Services.AddSingleton<RoutesOptions>(serviceProvider => {
                    return new RoutesOptions()
                    {
                        AppAssembly = entryAssembly,
                        DefaultLayout = defaultLayout
                    };
                });                
            }

            // MiddlewareBuilder
            applicationBuilder.AddHook("Routing", (application) =>
            {
                // BlazorRoute
                application
                    .MapRazorComponents<TApp>()
                    .AddInteractiveServerRenderMode()
                    .AddAdditionalAssemblies(MDP.Reflection.Assembly.FindAllApplicationAssembly().Where(o => o != entryAssembly).ToArray());
            });

            // Return
            return applicationBuilder;
        }
    }
}