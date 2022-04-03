using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
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
                webHostBuilder.UseSetting(WebHostDefaults.ApplicationKey, Assembly.GetEntryAssembly().FullName);
                webHostBuilder.UseSetting(WebHostDefaults.StartupAssemblyKey, typeof(TStartup).Assembly.FullName);
            });

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // MvcBuilder
                var mvcBuilder = services.AddMvc();
                if (mvcBuilder == null) throw new InvalidOperationException($"{nameof(mvcBuilder)}=null");

                // Service
                mvcBuilder.AddHttpContextAccessor();

                // Format
                mvcBuilder.AddUnicodeRanges();
                mvcBuilder.AddApiVersioning();
                mvcBuilder.AddContentNegotiation();

                // Module
                mvcBuilder.AddModuleLocation();
                mvcBuilder.AddModuleAsset();
                mvcBuilder.AddModuleApplicationPart();

                // Expand
                if (configureAction != null)
                {
                    configureAction(mvcBuilder);
                }
            });

            // Return
            return hostBuilder;
        }


        // Service
        private static void AddHttpContextAccessor(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // HttpContext
            mvcBuilder.Services.AddHttpContextAccessor();
        }

        // Format
        private static void AddUnicodeRanges(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // HtmlEncoder
            mvcBuilder.Services.AddSingleton<HtmlEncoder>
            (
                HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            );

            // JsonOptions
            mvcBuilder.AddJsonOptions(options =>
            {
                // Encoder
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
            });
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

        // Module
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

        private static void AddModuleAsset(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"*.Services.dll|*.Services.Views.dll")
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

            // AssetAssembly
            var assetAssemblyList = new List<Assembly>();
            foreach (var registeredAssembly in registeredAssemblyList)
            {
                if (assetAssemblyList.Contains(registeredAssembly) == false)
                {
                    assetAssemblyList.Add(registeredAssembly);
                }
            }
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (assetAssemblyList.Contains(moduleAssembly) == true)
                {
                    assetAssemblyList.Remove(moduleAssembly);
                }
                assetAssemblyList.Add(moduleAssembly);
            }            

            // FileProviderList
            var fileProviderList = new List<IFileProvider>();
            foreach (var assetAssembly in assetAssemblyList)
            {
                // FileProvider
                IFileProvider fileProvider = null;
                try
                {
                    fileProvider = new ManifestEmbeddedFileProvider(assetAssembly, @"wwwroot");
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
