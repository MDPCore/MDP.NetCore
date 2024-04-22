using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.BlazorCore.Maui
{
    public class MauiHostEnvironment : IHostEnvironment
    {
        // Constructors
        public MauiHostEnvironment(MauiEnvironmentVariables environmentVariables, Assembly entryAssembly)
        {
            #region Contracts

            if (environmentVariables == null) throw new ArgumentException($"{nameof(environmentVariables)}=null");
            if (entryAssembly == null) throw new ArgumentException($"{nameof(entryAssembly)}=null");

            #endregion

            // EnvironmentName
            var environmentName = environmentVariables.GetVariable("MAUI_ENVIRONMENT");
            if(string.IsNullOrEmpty(environmentName)==true) environmentName = Environments.Production;
            this.EnvironmentName = environmentName;

            // ApplicationName
            var applicationName = entryAssembly.GetName().Name;
            if (string.IsNullOrEmpty(applicationName) == true) new InvalidOperationException($"{nameof(applicationName)}=null");
            this.ApplicationName = applicationName;

            // ContentRootPath
            var contentRootPath = FileSystem.AppDataDirectory;
            if (string.IsNullOrEmpty(contentRootPath) == true) new InvalidOperationException($"{nameof(contentRootPath)}=null");
            this.ContentRootPath = contentRootPath;

            // ContentRootFileProvider
            this.ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
        }


        // Properties
        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; }

        public string ContentRootPath { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }


        // Methods
        public bool IsDevelopment()
        {
            // Return
            return this.IsEnvironment(Environments.Development);
        }
    }
}
