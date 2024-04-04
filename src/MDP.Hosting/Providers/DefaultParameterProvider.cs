using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    internal class DefaultParameterProvider : ParameterProvider
    {   
        // Methods
        protected override object GetValue(string parameterName, Type parameterType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");
            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");

            #endregion

            // Return
            return null;
        }
    }
}
