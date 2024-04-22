using System;
using System.Collections.Generic;

namespace MDP.Reflection
{
    public class DictionaryParameterProvider : ParameterProvider
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
        public override object GetValue(System.Type parameterType, string parameterName, bool hasDefaultValue = false, object defaultValue = null)
        {
            #region Contracts

            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");
            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // ParameterDictionary
            if (_parameterDictionary.ContainsKey(parameterName) == true)
            {
                var parameter = _parameterDictionary[parameterName];
                if (parameter != null && parameterType.IsInstanceOfType(parameter) == true) return parameter;
            }

            // Return
            return base.GetValue(parameterType, parameterName, hasDefaultValue, defaultValue);
        }
    }
}
