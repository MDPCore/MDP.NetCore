using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
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

            // ModuleConfigFileNameList
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

            // ModuleConfigFileList
            var moduleConfigFileList = CLK.IO.File.GetAllFile(string.Join("|", moduleConfigFileNameList));
            if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");

            // RegisterJsonFile
            foreach (var moduleConfigFile in moduleConfigFileList)
            {
                // Register
                configurationBuilder.AddJsonFile(moduleConfigFile.FullName);
            }

            // RegisterJsonEmptyElement
            var jsonEmptyElementDictionary = new Dictionary<string, string>();
            foreach (var moduleConfigFile in moduleConfigFileList)
            {
                // JsonDocument
                using (var jsonStream = moduleConfigFile.OpenRead())
                using (var jsonDocument = JsonDocument.Parse(jsonStream, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }))
                {
                    // NamespaceConfigElement
                    foreach (var namespaceConfigElement in jsonDocument.RootElement.EnumerateObject().Where(o=>o.Value.ValueKind == JsonValueKind.Object))
                    {
                        // ServiceConfigElement
                        foreach (var serviceConfigElement in namespaceConfigElement.Value.EnumerateObject())
                        {
                            // Require
                            if (serviceConfigElement.Value.ValueKind != JsonValueKind.Object) continue;
                            if (serviceConfigElement.Value.EnumerateObject().Count() != 0) continue;

                            // Add
                            var namespaceConfigKey = namespaceConfigElement.Name;
                            var serviceConfigKey = serviceConfigElement.Name;
                            var propertyName = "_______";
                            var propertyValue = "_______";
                            jsonEmptyElementDictionary.Add($"{namespaceConfigKey}:{serviceConfigKey}:{propertyName}", propertyValue);
                        }          
                    }
                }
            }
            configurationBuilder.AddInMemoryCollection(jsonEmptyElementDictionary);

            // Return
            return configurationBuilder;
        }
    }
}
