using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace MDP.BlazorCore.Maui
{
    public class MauiEnvironmentVariables
    {
        // Fields
        private readonly Dictionary<string, string> _variables = null;


        // Constructors
        public MauiEnvironmentVariables(string fileName = "Environment.ini")
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Default
            _variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // LoadVariables
            this.LoadVariables(fileName);
        }


        // Methods
        private void LoadVariables(string fileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // LoadFile
            FileSystem.OpenAppPackageFileAsync(fileName).ContinueWith(task =>
            {
                // Require
                if (task.IsFaulted == true) return;

                // FileContent
                using (var stream = task.Result)
                {
                    // Require
                    if (stream == null) return;

                    // FileContent
                    using (var reader = new StreamReader(stream))
                    {
                        // Require
                        if (reader == null) return;

                        // FileContent
                        foreach (var variableString in reader.ReadToEnd().Split('\n'))
                        {
                            // VariableStringList
                            var variableStringList = variableString.Split('=');
                            if (variableStringList.Length != 2) continue;

                            // VariableName
                            var variableName = variableStringList[0].Trim();
                            if (string.IsNullOrEmpty(variableName) == true) continue;

                            // VariableValue
                            var variableValue = variableStringList[1].Trim();
                            if (string.IsNullOrEmpty(variableValue) == true) continue;

                            // Add
                            _variables[variableName] = variableValue;
                        }
                    }
                }
            }).Wait();            
        }

        public string GetVariable(string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // Require
            if (_variables.ContainsKey(name) == false) return null;

            // Return
            return _variables[name];
        }
    }
}
