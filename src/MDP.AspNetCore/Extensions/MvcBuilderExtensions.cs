using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
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
    public static partial class MvcBuilderExtensions
    {
        // Methods
        public static IMvcBuilder RegisterModule(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // MvcBuilder
            {
                // MvcPart
                mvcBuilder.AddMvcPart();

                // MvcAsset
                mvcBuilder.AddMvcAsset();
            }

            // Return
            return mvcBuilder;
        }

        private static void AddMvcPart(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.FindAllAssembly();
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // RegisteredAssembly
            var registeredAssemblyList = new List<Assembly>();
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<CompiledRazorAssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));

            // PartAssembly
            var partAssemblyList = new List<Assembly>();
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (registeredAssemblyList.Contains(moduleAssembly) == false)
                {
                    partAssemblyList.Add(moduleAssembly);
                }
            }

            // ApplicationPart
            foreach (var partAssembly in partAssemblyList)
            {
                mvcBuilder.AddApplicationPart(partAssembly);
            }
        }

        private static void AddMvcAsset(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.FindAllAssembly();
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
                if (assetAssemblyList.Contains(moduleAssembly) == false)
                {
                    assetAssemblyList.Add(moduleAssembly);
                }
            }

            // FileProviderList
            var fileProviderList = new List<IFileProvider>();
            foreach (var assetAssembly in assetAssemblyList)
            {
                // FileProvider
                IFileProvider? fileProvider = null;
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
    }
}
