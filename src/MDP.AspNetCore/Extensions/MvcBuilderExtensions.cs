using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using MDP.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace MDP
{
    public static class MvcBuilderExtensions
    {
        // Methods
        public static IMvcBuilder AddMDP(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));

            #endregion

            // WebHostEnvironment
            mvcBuilder.Services.AddService<IWebHostEnvironment>((hostEnvironment) =>
            {
                // ModuleWebAssets
                hostEnvironment.RegisterModuleWebAsset();
            });

            // MvcBuilder
            {
                // ModuleApplicationPart
                mvcBuilder.RegisterModuleApplicationPart();
            }

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

            // Return
            return mvcBuilder;
        }

        private static void RegisterModuleWebAsset(this IWebHostEnvironment hostEnvironment, string moduleAssemblyFileName = @"*.Services.dll")
        {
            #region Contracts

            if (hostEnvironment == null) throw new ArgumentException(nameof(hostEnvironment));
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
            fileProviderList.Insert(0, hostEnvironment.WebRootFileProvider);

            // Register
            hostEnvironment.WebRootFileProvider = new CompositeFileProvider
            (
                fileProviderList
            );
        }

        private static void RegisterModuleApplicationPart(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"*.Services.dll|*.Services.Views.dll")
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