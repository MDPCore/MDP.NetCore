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
    public class MvcRegister
    {
        // Methods
        public static IMvcBuilder RegisterModule(IMvcBuilder mvcBuilder)
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

            // RegisterMvcPart
            {
                // PartAssembly
                var partAssemblyList = new List<Assembly>();
                foreach (var moduleAssembly in moduleAssemblyList)
                {
                    if (registeredAssemblyList.Contains(moduleAssembly) == false)
                    {
                        partAssemblyList.Add(moduleAssembly);
                    }
                }

                // Register
                foreach (var partAssembly in partAssemblyList)
                {
                    // AddApplicationPart
                    mvcBuilder.AddApplicationPart(partAssembly);
                }
            }

            // RegisterMvcAsset
            {
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

                // AssetProvider
                var assetProviderList = new List<IFileProvider>();
                foreach (var assetAssembly in assetAssemblyList)
                {
                    // AssetProvider
                    IFileProvider assetProvider = null;
                    try
                    {
                        assetProvider = new ManifestEmbeddedFileProvider(assetAssembly, @"wwwroot");
                    }
                    catch
                    {
                        assetProvider = null;
                    }

                    // Add
                    if (assetProvider != null)
                    {
                        assetProviderList.Add(assetProvider);
                    }
                }

                // Register
                mvcBuilder.Services.AddOptions<StaticFileOptions>().Configure<IWebHostEnvironment>((options, hostEnvironment) =>
                {
                    // RootFileProvider
                    if (hostEnvironment.WebRootFileProvider != null)
                    {
                        assetProviderList.Insert(0, hostEnvironment.WebRootFileProvider);
                    }

                    // Attach
                    options.FileProvider = new CompositeFileProvider
                    (
                        assetProviderList
                    );
                });
            }

            // Return
            return mvcBuilder;
        }
    }
}
