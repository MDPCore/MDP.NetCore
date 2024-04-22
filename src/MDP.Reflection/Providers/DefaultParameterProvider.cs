using System;
using System.Collections.Generic;

namespace MDP.Reflection
{
    public class DefaultParameterProvider : ParameterProvider
    {
        // Methods
        public override object GetValue(System.Type parameterType, string parameterName, bool hasDefaultValue = false, object defaultValue = null)
        {
            #region Contracts

            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");
            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // Default
            {

            }

            // Return
            return base.GetValue(parameterType, parameterName, hasDefaultValue, defaultValue);
        }
    }
}
