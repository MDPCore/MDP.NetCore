using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureAspNetCore<TStartup>(this IHostBuilder hostBuilder, Action<IMvcBuilder> configureAction = null) where TStartup : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Defaults
            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                // Startup
                webHostBuilder.UseStartup<TStartup>();
            });

            // Services
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                // MvcBuilder
                var mvcBuilder = services.AddMvc();
                if (mvcBuilder == null) throw new InvalidOperationException($"{nameof(mvcBuilder)}=null");

                // Module
                mvcBuilder.AddModule();

                // ApiVersioning
                mvcBuilder.AddApiVersioning();

                // ContentNegotiation
                mvcBuilder.AddContentNegotiation();

                // Expand
                if (configureAction != null)
                {
                    configureAction(mvcBuilder);
                }
            });

            // Return
            return hostBuilder;
        }


        private static void AddApiVersioning(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // ApiVersioning
            mvcBuilder.Services.AddApiVersioning((options) =>
            {
                // Report
                options.ReportApiVersions = true;

                // Reader
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                // Default
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        private static void AddContentNegotiation(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // MvcOptions
            mvcBuilder.AddMvcOptions((options) =>
            {
                // ApiOutput
                {
                    // NotAcceptable
                    options.ReturnHttpNotAcceptable = true;

                    // BrowserAcceptHeader
                    options.RespectBrowserAcceptHeader = true;

                    // OutputFormatters
                    {
                        // XML
                        options.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                        // Null
                        options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                        options.OutputFormatters.Insert(0, new NullOutputFormatter());
                    }
                }
            });
        }


        private static void AddModule(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // Location
            mvcBuilder.AddModuleLocation();

            // Asset
            mvcBuilder.AddModuleAsset();

            // ApplicationPart
            mvcBuilder.AddModuleApplicationPart();
        }

        private static void AddModuleLocation(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // RazorOptions
            mvcBuilder.AddRazorOptions((options) =>
            {
                // ViewLocation
                {
                    // Area
                    options.AreaViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");
                }
            });
        }

        private static void AddModuleAsset(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"*.Services.dll")
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // FileProviderList
            var fileProviderList = new List<IFileProvider>();
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                // FileProvider
                IFileProvider fileProvider = null;
                try
                {
                    fileProvider = new ManifestEmbeddedFileProvider(moduleAssembly, @"wwwroot");
                }
                catch
                {
                    fileProvider = null;
                }

                // Add
                if (fileProvider != null)
                {
                    fileProviderList.Add(fileProvider);
                }
            }

            // StaticFileOptions
            mvcBuilder.Services.AddOptions<StaticFileOptions>().Configure<IWebHostEnvironment>((options, hostEnvironment) =>
            {
                // FileProvider
                if (hostEnvironment.WebRootFileProvider != null)
                {
                    fileProviderList.Insert(0, hostEnvironment.WebRootFileProvider);
                }

                // Attach
                options.FileProvider = new CompositeFileProvider
                (
                    fileProviderList
                );
            });
        }

        private static void AddModuleApplicationPart(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"*.Services.dll|*.Services.Views.dll")
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // RegisteredAssembly
            var registeredAssemblyList = new List<Assembly>();
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<CompiledRazorAssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));

            // ApplicationPart
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (registeredAssemblyList.Contains(moduleAssembly) == false)
                {
                    mvcBuilder.AddApplicationPart(moduleAssembly);
                }
            }
        }
    }
}
