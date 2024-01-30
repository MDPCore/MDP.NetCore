using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    internal class DictionaryParameterProvider : ParameterProvider
    {
        // Fields
        private readonly Dictionary<string, object> _parameterDictionary = null;


        // Constructors
        public DictionaryParameterProvider(Dictionary<string, object> parameterDictionary)
        {
            #region Contracts

            if (parameterDictionary == null) throw new ArgumentException($"{nameof(parameterDictionary)}=null");

            #endregion
                       
            // Default
            _parameterDictionary = new Dictionary<string, object>(parameterDictionary, StringComparer.OrdinalIgnoreCase);
        }


        // Methods
        protected override bool ExistValue(string parameterName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // Result
            var result = _parameterDictionary.ContainsKey(parameterName);

            // Return
            return result;
        }

        protected override object GetValue(string parameterName, Type parameterType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");
            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");

            #endregion

            // Require
            if (_parameterDictionary.ContainsKey(parameterName) == false) return null;

            // Parameter
            object parameter = _parameterDictionary[parameterName];
            if (parameter == null) return null;
            if (parameterType.IsInstanceOfType(parameter) == false) return null;

            // Return
            return parameter;
        }
    }
}
