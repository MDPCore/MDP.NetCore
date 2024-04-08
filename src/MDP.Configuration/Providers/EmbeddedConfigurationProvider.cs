using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Configuration
{
    public class EmbeddedConfigurationProvider : ConfigurationProvider
    {
        // Fields
        private readonly Assembly _assembly = null;

        private readonly string _environmentName = null;

        private readonly string _configDirectoryName = null;


        // Constructors
        public EmbeddedConfigurationProvider(Assembly assembly, string environmentName, string configDirectoryName = "config")
        {
            #region Contracts

            if (assembly == null) throw new ArgumentException($"{nameof(assembly)}=null");
            if (string.IsNullOrEmpty(environmentName) == true) throw new ArgumentException($"{nameof(environmentName)}=null");
            if (string.IsNullOrEmpty(configDirectoryName) == true) throw new ArgumentException($"{nameof(configDirectoryName)}=null");

            #endregion

            // Default
            _assembly = assembly;
            _environmentName = environmentName;
            _configDirectoryName = configDirectoryName;
        }


        // Methods
        public IEnumerable<Stream> GetAllJsonStream()
        {
            // Namespace
            var @namespace = _assembly.GetName().Name;
            if (string.IsNullOrEmpty(@namespace) == true) throw new InvalidDataException($"{nameof(@namespace)}=null");

            // ResourceNameDictionary
            var resourceNameDictionary = _assembly.GetManifestResourceNames()?.Where(o => o.EndsWith($".json", StringComparison.OrdinalIgnoreCase) == true).ToDictionary(name => name, name => name, StringComparer.OrdinalIgnoreCase);
            if (resourceNameDictionary == null) throw new InvalidOperationException($"{nameof(resourceNameDictionary)}=null");

            // EntryDirectoryPath
            var entryDirectoryPath = @namespace;
            if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException($"{nameof(entryDirectoryPath)}=null");
            if (string.IsNullOrEmpty(entryDirectoryPath) == false)
            {
                // appsettings.json
                if (resourceNameDictionary.ContainsKey($"{entryDirectoryPath}.appsettings.json") == true)
                {
                    yield return _assembly.GetManifestResourceStream($"{entryDirectoryPath}.appsettings.json");
                }

                // appsettings.{environmentName}.json
                if (resourceNameDictionary.ContainsKey($"{entryDirectoryPath}.appsettings.{_environmentName}.json") == true)
                {
                    yield return _assembly.GetManifestResourceStream($"{entryDirectoryPath}.appsettings.{_environmentName}.json");
                }
            }

            // ConfigDirectoryPath
            var configDirectoryPath = $"{entryDirectoryPath}.{_configDirectoryName}";
            if (string.IsNullOrEmpty(configDirectoryPath) == true) throw new InvalidOperationException($"{nameof(configDirectoryPath)}=null");
            if (string.IsNullOrEmpty(configDirectoryPath) == false)
            {
                // appsettings.json
                if (resourceNameDictionary.ContainsKey($"{configDirectoryPath}.appsettings.json") == true)
                {
                    yield return _assembly.GetManifestResourceStream($"{configDirectoryPath}.appsettings.json");
                }

                // appsettings.{environmentName}.json
                if (resourceNameDictionary.ContainsKey($"{configDirectoryPath}.appsettings.{_environmentName}.json") == true)
                {
                    yield return _assembly.GetManifestResourceStream($"{configDirectoryPath}.appsettings.{_environmentName}.json");
                }
            }

            // EnvironmentDirectoryPath
            var environmentDirectoryPath = $"{configDirectoryPath}.{_environmentName}";
            if (string.IsNullOrEmpty(environmentDirectoryPath) == true) throw new InvalidOperationException($"{nameof(environmentDirectoryPath)}=null");
            if (string.IsNullOrEmpty(environmentDirectoryPath) == false)
            {
                // *.json
                foreach (var resourceName in resourceNameDictionary.Values)
                {
                    // Require
                    if (resourceName.StartsWith($"{environmentDirectoryPath}.", StringComparison.OrdinalIgnoreCase) == false) continue;

                    // Return
                    yield return _assembly.GetManifestResourceStream(resourceName);
                }
            }
        }
    }
}
