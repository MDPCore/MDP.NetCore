using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text.Json;

namespace MDP.Hosting
{
    public static class ConfigurationBuilderExtensions
    {
        // Methods   
        public static IConfigurationBuilder RegisterModule(this IConfigurationBuilder configurationBuilder, IHostEnvironment hostEnvironment, string moduleConfigDirectoryPath = "config", string moduleConfigFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException($"{nameof(configurationBuilder)}=null");
            if (hostEnvironment == null) throw new ArgumentException($"{nameof(hostEnvironment)}=null");
            if (string.IsNullOrEmpty(moduleConfigDirectoryPath) == true) throw new ArgumentException($"{nameof(moduleConfigDirectoryPath)}=null");
            if (string.IsNullOrEmpty(moduleConfigFileName) == true) throw new ArgumentException($"{nameof(moduleConfigFileName)}=null");

            #endregion

            // ModuleConfigFileNameList
            var moduleConfigFileNameList = new List<string>();
            {
                // ConfigDirectoryPathList
                var configDirectoryPathList = new List<string>();
                {
                    // Default
                    configDirectoryPathList.Add(String.Empty);
                    configDirectoryPathList.Add(moduleConfigDirectoryPath);

                    // Environment
                    if (hostEnvironment.IsProduction() == true) configDirectoryPathList.Add(Path.Combine(moduleConfigDirectoryPath, "prod"));
                    if (hostEnvironment.IsStaging() == true) configDirectoryPathList.Add(Path.Combine(moduleConfigDirectoryPath, "stage"));
                    if (hostEnvironment.IsDevelopment() == true) configDirectoryPathList.Add(Path.Combine(moduleConfigDirectoryPath, "dev"));
                }

                // ConfigFileNameList
                var configFileNameList = new List<string>();
                {
                    // Default
                    configFileNameList.Add(moduleConfigFileName);

                    // AppConfigFileName
                    var appConfigFileName = "appsettings.json";
                    if (string.IsNullOrEmpty(appConfigFileName) == true) throw new InvalidOperationException($"{nameof(appConfigFileName)}=null");
                    configFileNameList.Add(appConfigFileName);

                    // EntryConfigFileName
                    var entryConfigFileName = Assembly.GetEntryAssembly()?.GetName().Name + ".json";
                    if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new InvalidOperationException($"{nameof(entryConfigFileName)}=null");
                    configFileNameList.Add(entryConfigFileName);
                }

                // Combine
                foreach (var configDirectoryPath in configDirectoryPathList)
                {
                    foreach (var configFileName in configFileNameList)
                    {
                        // Add
                        moduleConfigFileNameList.Add(Path.Combine(configDirectoryPath, configFileName));
                    }
                }
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
    }
}
