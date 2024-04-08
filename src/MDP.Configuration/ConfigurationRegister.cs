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
        public static void RegisterModule(IConfigurationBuilder configurationBuilder, ConfigurationProvider configurationProvider)
        {
            #region Contracts

            if (configurationBuilder == null) throw new ArgumentException($"{nameof(configurationBuilder)}=null");
            if (configurationProvider == null) throw new ArgumentException($"{nameof(configurationProvider)}=null");

            #endregion

            // ConfigurationProvider
            foreach (var jsonStream in configurationProvider.GetAllJsonStream())
            {
                // JsonEmptyElementDictionary
                var jsonEmptyElementDictionary = new Dictionary<string, string>();
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
                configurationBuilder.AddInMemoryCollection(jsonEmptyElementDictionary);

                // JsonStream
                jsonStream.Position = 0;
                configurationBuilder.AddJsonStream(jsonStream);
            }
        }
    }
}