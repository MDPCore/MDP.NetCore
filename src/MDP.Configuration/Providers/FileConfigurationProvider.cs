using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Configuration
{
    public class FileConfigurationProvider : ConfigurationProvider
    {
        // Fields
        private readonly string _environmentName = null;

        private readonly string _configDirectoryName = null;


        // Constructors
        public FileConfigurationProvider(string environmentName, string configDirectoryName = "config")
        {
            #region Contracts

            if (string.IsNullOrEmpty(environmentName) == true) throw new ArgumentException($"{nameof(environmentName)}=null");
            if (string.IsNullOrEmpty(configDirectoryName) == true) throw new ArgumentException($"{nameof(configDirectoryName)}=null");

            #endregion

            // Default
            _environmentName = environmentName;
            _configDirectoryName = configDirectoryName;
        }


        // Methods
        public IEnumerable<Stream> GetAllJsonStream()
        {
            // EntryDirectoryPath
            var entryDirectoryPath = MDP.IO.Directory.GetEntryDirectoryPath();
            if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException($"{nameof(entryDirectoryPath)}=null");
            if (string.IsNullOrEmpty(entryDirectoryPath) == false)
            {
                // appsettings.json
                if (System.IO.File.Exists(Path.Combine(entryDirectoryPath, "appsettings.json")) == true)
                {
                    yield return File.OpenRead(Path.Combine(entryDirectoryPath, "appsettings.json"));
                }

                // appsettings.{environmentName}.json
                if (System.IO.File.Exists(Path.Combine(entryDirectoryPath, $"appsettings.{_environmentName}.json")) == true)
                {
                    yield return File.OpenRead(Path.Combine(entryDirectoryPath, $"appsettings.{_environmentName}.json"));
                }
            }

            // ConfigDirectoryPath
            var configDirectoryPath = Path.Combine(entryDirectoryPath, _configDirectoryName);
            if (string.IsNullOrEmpty(configDirectoryPath) == true) throw new InvalidOperationException($"{nameof(configDirectoryPath)}=null");
            if (string.IsNullOrEmpty(configDirectoryPath) == false)
            {
                // appsettings.json
                if (System.IO.File.Exists(Path.Combine(configDirectoryPath, "appsettings.json")) == true)
                {
                    yield return File.OpenRead(Path.Combine(configDirectoryPath, "appsettings.json"));
                }

                // appsettings.{environmentName}.json
                if (System.IO.File.Exists(Path.Combine(configDirectoryPath, $"appsettings.{_environmentName}.json")) == true)
                {
                    yield return File.OpenRead(Path.Combine(configDirectoryPath, $"appsettings.{_environmentName}.json"));
                }
            }

            // EnvironmentDirectoryPath
            var environmentDirectoryPath = Path.Combine(configDirectoryPath, _environmentName);
            if (string.IsNullOrEmpty(environmentDirectoryPath) == true) throw new InvalidOperationException($"{nameof(environmentDirectoryPath)}=null");
            if (string.IsNullOrEmpty(environmentDirectoryPath) == false)
            {
                // *.json
                foreach (var jsonFilePath in MDP.IO.File.GetAllFilePath($"*.json", environmentDirectoryPath))
                {
                    yield return File.OpenRead(jsonFilePath);
                }
            }
        }
    }
}
