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

            // moduleConfigFileNameList
            var moduleConfigFileNameList = new List<string>();
            {
                // ModuleConfigFileName
                moduleConfigFileNameList.Add(moduleConfigFileName);

                // AppConfigFileName
                var appConfigFileName = "appsettings.json";
                if (string.IsNullOrEmpty(appConfigFileName) == true) throw new InvalidOperationException($"{nameof(appConfigFileName)}=null");
                moduleConfigFileNameList.Add(appConfigFileName);

                // EntryConfigFileName
                var entryConfigFileName = Assembly.GetEntryAssembly().GetName().Name + ".json";
                if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new InvalidOperationException($"{nameof(entryConfigFileName)}=null");
                moduleConfigFileNameList.Add(entryConfigFileName);
            }

            // ModuleConfigFile
            var moduleConfigFileList = CLK.IO.File.GetAllFile(string.Join("|", moduleConfigFileNameList));
            if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");

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
