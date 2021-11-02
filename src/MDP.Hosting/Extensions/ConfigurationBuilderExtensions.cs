using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public static class ConfigurationBuilderExtensions
    {
        // Methods   
        public static IConfigurationBuilder RegisterModule(this IConfigurationBuilder configurationBuilder, string moduleConfigFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException(nameof(configurationBuilder));
            if (string.IsNullOrEmpty(moduleConfigFileName) == true) throw new ArgumentException(nameof(moduleConfigFileName));

            #endregion

            // ModuleConfigFile
            var moduleConfigFileList = CLK.IO.File.GetAllFile(moduleConfigFileName);
            if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");

            // EntryConfigFile
            var entryConfigFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "json");
            if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new ArgumentException(nameof(entryConfigFileName));
            var entryConfigFile = new FileInfo(entryConfigFileName);
            if (entryConfigFile.Exists == true) moduleConfigFileList.RemoveAll(moduleConfigFile => moduleConfigFile.FullName == entryConfigFile.FullName);
            if (entryConfigFile.Exists == true) moduleConfigFileList.Add(entryConfigFile);

            // Register
            foreach (var moduleConfigFile in moduleConfigFileList)
            {
                configurationBuilder.AddJsonFile(moduleConfigFile.FullName);
            }

            // Return
            return configurationBuilder;
        }
    }
}
