using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.BlazorCore
{
    public class RoutesOptions
    {
        // Properties
        public Assembly AppAssembly { get; set; }

        public Type DefaultLayout { get; set; }

        public string LoginPath { get; set; } = "/Account/Login";

        public string AccessDeniedPath { get; set; } = "/Account/AccessDenied";


        // Methods
        public Assembly CreateAppAssembly()
        {
            // AppAssembly
            if (this.AppAssembly != null) return this.AppAssembly;

            // EntryAssembly
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null) return entryAssembly;

            // Return
            return null;
        }

        public Assembly[] CreateAdditionalAssemblies()
        {
            // AssemblyList
            var assemblyList = MDP.Reflection.Assembly.FindAllApplicationAssembly();
            if (assemblyList == null) return Array.Empty<Assembly>();

            // AppAssembly
            var appAssembly = this.CreateAppAssembly();
            if (appAssembly != null) assemblyList.Remove(appAssembly);

            // Return
            return assemblyList.ToArray();
        }

        public Type CreateDefaultLayout()
        {
            // Required
            if (this.DefaultLayout == null) return null;

            // Return
            return this.DefaultLayout;
        }
    }
}
