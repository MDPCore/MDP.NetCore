using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace MDP.Hosting
{
    public static class ConfigurationBuilderExtensions
    {
        // Methods
        public static void AddModuleConfiguration(this IConfigurationBuilder configuration, string moduleConfigFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(moduleConfigFileName) == true) throw new ArgumentException(nameof(moduleConfigFileName));

            #endregion

            // ModuleConfigFile
            var moduleConfigFileList = CLK.IO.File.GetAllFile(moduleConfigFileName);
            if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");
            moduleConfigFileList.ForEach(moduleConfigFile => configuration.AddJsonFile(moduleConfigFile.FullName));

            // EntryConfigFile
            var entryConfigFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "json");
            if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new ArgumentException(nameof(entryConfigFileName));
            if (File.Exists(entryConfigFileName) == true) configuration.AddJsonFile(entryConfigFileName);
        }
    }
}