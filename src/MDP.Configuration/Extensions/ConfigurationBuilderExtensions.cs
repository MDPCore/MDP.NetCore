using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Linq;

namespace MDP.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        // Methods   
        public static IConfigurationBuilder RegisterModule(this IConfigurationBuilder configurationBuilder, string environmentName, string configDirectoryName = "config")
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException($"{nameof(configurationBuilder)}=null");
            if (string.IsNullOrEmpty(environmentName) == true) throw new ArgumentException($"{nameof(environmentName)}=null");

            #endregion
                        
            // ConfigFilePathList
            var configFilePathList = new List<string>();
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException($"{nameof(entryDirectoryPath)}=null");

                // ConfigDirectoryPath
                var configDirectoryPath = Path.Combine(entryDirectoryPath, configDirectoryName);
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException($"{nameof(configDirectoryPath)}=null");

                // EnvironmentDirectoryPath
                var environmentDirectoryPath = Path.Combine(configDirectoryPath, environmentName);
                if (string.IsNullOrEmpty(environmentDirectoryPath) == true) throw new InvalidOperationException($"{nameof(environmentDirectoryPath)}=null");

                // ShortEnvironmentDirectoryPath
                var shortEnvironmentDirectoryPath = string.Empty;
                if (String.Equals(environmentName, "Production", StringComparison.OrdinalIgnoreCase) == true) shortEnvironmentDirectoryPath = Path.Combine(configDirectoryPath, "prod");
                if (String.Equals(environmentName, "Staging", StringComparison.OrdinalIgnoreCase) == true) shortEnvironmentDirectoryPath = Path.Combine(configDirectoryPath, "stage");
                if (String.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase) == true) shortEnvironmentDirectoryPath = Path.Combine(configDirectoryPath, "dev");

                // EntryDirectoryPath
                {
                    // appsettings.json
                    if (System.IO.File.Exists(Path.Combine(entryDirectoryPath, "appsettings.json")) == true)
                    {
                        configFilePathList.Add(Path.Combine(entryDirectoryPath, "appsettings.json"));
                    }

                    // *.{environmentName}.json
                    configFilePathList.AddRange(SearchAllFilePath(entryDirectoryPath).Where(file => file.EndsWith($"{environmentName}.json", StringComparison.OrdinalIgnoreCase)));
                }

                // ConfigDirectoryPath
                {
                    // appsettings.json
                    if (System.IO.File.Exists(Path.Combine(configDirectoryPath, "appsettings.json")) == true)
                    {
                        configFilePathList.Add(Path.Combine(configDirectoryPath, "appsettings.json"));
                    }

                    // *.{environmentName}.json
                    configFilePathList.AddRange(SearchAllFilePath(configDirectoryPath).Where(file => file.EndsWith($"{environmentName}.json", StringComparison.OrdinalIgnoreCase)));
                }

                // EnvironmentDirectoryPath
                {
                    // *.json
                    configFilePathList.AddRange(SearchAllFilePath(environmentDirectoryPath));
                }

                // ShortEnvironmentDirectoryPath
                if (string.IsNullOrEmpty(shortEnvironmentDirectoryPath) == false)
                {
                    // *.json
                    configFilePathList.AddRange(SearchAllFilePath(shortEnvironmentDirectoryPath));
                }
            }

            // RegisterJsonFile
            foreach (var configFilePath in configFilePathList)
            {
                // Register
                configurationBuilder.AddJsonFile(configFilePath);
            }

            // RegisterJsonEmptyElement
            var jsonEmptyElementDictionary = new Dictionary<string, string>();
            foreach (var configFilePath in configFilePathList)
            {
                // JsonDocument
                using (var jsonStream = new FileInfo(configFilePath).OpenRead())
                using (var jsonDocument = JsonDocument.Parse(jsonStream, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }))
                {
                    // NamespaceConfigElement
                    foreach (var namespaceConfigElement in jsonDocument.RootElement.EnumerateObject().Where(o => o.Value.ValueKind == JsonValueKind.Object))
                    {
                        // ServiceConfigElement
                        foreach (var serviceConfigElement in namespaceConfigElement.Value.EnumerateObject())
                        {
                            // EmptyServiceConfigElement
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

            // Require
            if (System.IO.Directory.Exists(directoryPath) == false) return new List<string>();

            // Search
            var filePathList = Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            if (filePathList == null) throw new InvalidOperationException($"{nameof(filePathList)}=null");

            // Return
            return filePathList.ToList();
        }
    }
}
