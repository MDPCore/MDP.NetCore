using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace MDP.Configuration
{
    public class ConfigurationRegister
    {
        // Methods   
        public static IConfigurationBuilder RegisterModule(IConfigurationBuilder configurationBuilder, string environmentName, string configDirectoryName = "config")
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException($"{nameof(configurationBuilder)}=null");
            if (string.IsNullOrEmpty(environmentName) == true) throw new ArgumentException($"{nameof(environmentName)}=null");

            #endregion

            // RegisterJsonFilePath
            var jsonFilePathList = new List<string>();
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException($"{nameof(entryDirectoryPath)}=null");
                if (string.IsNullOrEmpty(entryDirectoryPath) == false)
                {
                    // appsettings.json
                    if (System.IO.File.Exists(Path.Combine(entryDirectoryPath, "appsettings.json")) == true)
                    {
                        jsonFilePathList.Add(Path.Combine(entryDirectoryPath, "appsettings.json"));
                    }

                    // *.{environmentName}.json
                    jsonFilePathList.AddRange(SearchAllFilePath(entryDirectoryPath).Where(file => file.EndsWith($"{environmentName}.json", StringComparison.OrdinalIgnoreCase)));
                }

                // ConfigDirectoryPath
                var configDirectoryPath = Path.Combine(entryDirectoryPath, configDirectoryName);
                if (string.IsNullOrEmpty(configDirectoryPath) == true) throw new InvalidOperationException($"{nameof(configDirectoryPath)}=null");
                if (string.IsNullOrEmpty(configDirectoryPath) == false)
                {
                    // appsettings.json
                    if (System.IO.File.Exists(Path.Combine(configDirectoryPath, "appsettings.json")) == true)
                    {
                        jsonFilePathList.Add(Path.Combine(configDirectoryPath, "appsettings.json"));
                    }

                    // *.{environmentName}.json
                    jsonFilePathList.AddRange(SearchAllFilePath(configDirectoryPath).Where(file => file.EndsWith($"{environmentName}.json", StringComparison.OrdinalIgnoreCase)));
                }

                // EnvironmentDirectoryPath
                var environmentDirectoryPath = Path.Combine(configDirectoryPath, environmentName);
                if (string.IsNullOrEmpty(environmentDirectoryPath) == true) throw new InvalidOperationException($"{nameof(environmentDirectoryPath)}=null");
                if (string.IsNullOrEmpty(environmentDirectoryPath) == false)
                {
                    // *.json
                    jsonFilePathList.AddRange(SearchAllFilePath(environmentDirectoryPath));
                }

                // Register
                foreach (var jsonFilePath in jsonFilePathList)
                {
                    // AddJsonFile
                    configurationBuilder.AddJsonFile(jsonFilePath);
                }
            }

            // RegisterJsonEmptyElement
            var jsonEmptyElementDictionary = new Dictionary<string, string>();
            foreach (var configFilePath in jsonFilePathList)
            {
                // JsonDocument
                using (var jsonStream = new FileInfo(configFilePath).OpenRead())
                using (var jsonDocument = JsonDocument.Parse(jsonStream, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }))
                {
                    // NamespaceConfigElement
                    foreach (var namespaceConfigElement in jsonDocument.RootElement.EnumerateObject().Where(o => o.Value.ValueKind == JsonValueKind.Object))
                    {
                        // EmptyServiceConfigElement
                        foreach (var serviceConfigElement in namespaceConfigElement.Value.EnumerateObject())
                        {
                            // Require
                            if (serviceConfigElement.Value.ValueKind != JsonValueKind.Object) continue;
                            if (serviceConfigElement.Value.EnumerateObject().Any() == true) continue;

                            // Add
                            var namespaceConfigKey = namespaceConfigElement.Name;
                            var serviceConfigKey = serviceConfigElement.Name;
                            var propertyName = "_______";
                            var propertyValue = "_______";
                            jsonEmptyElementDictionary.Add($"{namespaceConfigKey}:{serviceConfigKey}:{propertyName}", propertyValue);
                        }

                        // EmptyNamespaceConfigElement
                        {
                            // Require
                            if (namespaceConfigElement.Value.ValueKind != JsonValueKind.Object) continue;
                            if (namespaceConfigElement.Value.EnumerateObject().Any() == true) continue;

                            // Add
                            var namespaceConfigKey = namespaceConfigElement.Name;
                            var propertyName = "_______";
                            var propertyValue = "_______";
                            jsonEmptyElementDictionary.Add($"{namespaceConfigKey}:{propertyName}", propertyValue);
                        }
                    }
                }
            }
            configurationBuilder.AddInMemoryCollection(jsonEmptyElementDictionary);

            // Return
            return configurationBuilder;
        }

        private static List<string> SearchAllFilePath(string directoryPath, string searchPattern = "*.json")
        {
            #region Contracts

            if (string.IsNullOrEmpty(directoryPath) == true) throw new ArgumentException($"{nameof(directoryPath)}=null");
            if (string.IsNullOrEmpty(searchPattern) == true) throw new ArgumentException($"{nameof(searchPattern)}=null");

            #endregion

            // Search
            var fileInfoList = CLK.IO.File.GetAllFile(searchPattern, directoryPath);
            if (fileInfoList == null) throw new InvalidOperationException($"{nameof(fileInfoList)}=null");

            // Return
            return fileInfoList.Select(o => o.FullName).ToList();
        }
    }
}