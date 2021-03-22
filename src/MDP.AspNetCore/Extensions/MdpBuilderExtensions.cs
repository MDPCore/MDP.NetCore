using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static class MdpBuilderExtensions
    {
        // Methods
        public static MdpBuilder AddMvc(this MdpBuilder mdpBuilder)
        {
            #region Contracts

            if (mdpBuilder == null) throw new ArgumentException(nameof(mdpBuilder));

            #endregion

            // ConfigureServices
            mdpBuilder.HostBuilder.ConfigureServices((hostContext, services) =>
            {
                // MvcBuilder
                var mvcBuilder = services.AddMvc();
                if (mvcBuilder == null) throw new InvalidOperationException($"{nameof(mvcBuilder)}=null");

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

                // RazorOptions
                mvcBuilder.AddRazorOptions((options) =>
                {
                    // ViewLocationFormats
                    {
                        // Area
                        options.AreaViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
                        options.AreaViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");
                    }
                });

                // ApiVersion
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

                // ModuleWebAsset
                mvcBuilder.AddModuleWebAsset();

                // ModuleApplicationPart
                mvcBuilder.AddModuleApplicationPart();
            });

            // Return
            return mdpBuilder;
        }

        private static void AddModuleWebAsset(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"*.Services.dll")
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
            //fileProviderList.Insert(0, hostEnvironment.WebRootFileProvider);

            // Register
            mvcBuilder.Services.Configure<StaticFileOptions>((options) =>
            {
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

            // AddApplicationPart
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
