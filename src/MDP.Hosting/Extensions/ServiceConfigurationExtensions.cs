using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDP
{
    public static partial class ServiceConfigurationExtensions
    {
        // Methods
        public static TValue GetValue<TValue>(this IServiceConfiguration configuration, string valueKey)
           where TValue : notnull
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(valueKey) == true) throw new ArgumentException(nameof(valueKey));

            #endregion

            // Result
            string valueString = null;

            // ServiceValueString
            var serviceValueString = configuration.GetServiceValueString(valueKey);
            if (string.IsNullOrEmpty(serviceValueString) == false) valueString = serviceValueString;
            if (string.IsNullOrEmpty(serviceValueString) == true) return default(TValue);

            // GlobalValueString
            var globalValueString = configuration.GetGlobalValueString(serviceValueString);
            if (string.IsNullOrEmpty(globalValueString) == false) valueString = globalValueString;

            // ValueString
            if (string.IsNullOrEmpty(valueString) == true)
            {
                return default(TValue);
            }
            return (TValue)Convert.ChangeType(valueString, typeof(TValue));
        }

        private static string GetServiceValueString(this IServiceConfiguration configuration, string valueKey)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(valueKey) == true) throw new ArgumentException(nameof(valueKey));

            #endregion

            // ServiceValueString
            var serviceValueString = configuration.ServiceSection?.GetValue<string>(valueKey);
            if (string.IsNullOrEmpty(serviceValueString) == true) return null;

            // Return
            return serviceValueString;
        }

        private static string GetGlobalValueString(this IServiceConfiguration configuration, string valueString)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(valueString) == true) throw new ArgumentException(nameof(valueString));

            #endregion

            // GlobalSectionKey
            var globalSectionKey = Regex.Match(valueString, @"(?<=^\[).+?(?=\])").Value;
            if (string.IsNullOrEmpty(globalSectionKey) == true) return null;

            // GlobalSection
            var globalSection = configuration.RootSection?.GetSection(globalSectionKey);
            if (globalSection == null) return null;

            // GlobalValueKey
            var globalValueKey = valueString.Remove(0, globalSectionKey.Length + 2);
            if (string.IsNullOrEmpty(globalValueKey) == true) return null;

            // GlobalValueString
            var globalValueString = globalSection.GetValue<string>(globalValueKey);
            if (string.IsNullOrEmpty(globalValueString) ==true) return null;

            // Return
            return globalValueString;
        }
    }
}