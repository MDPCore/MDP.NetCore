using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public class Configuration<TService> 
        where TService : class
    {
        // Fields
        private IConfiguration _rootSection = null;

        private IConfiguration _serviceSection = null;


        // Constructors
        public Configuration(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // RootSection
            _rootSection = configuration;

            // ServiceSection
            {
                // ServiceType
                var serviceType = typeof(TService);
                if (serviceType == null) throw new InvalidOperationException($"{nameof(serviceType)}=null");

                // ServiceSectionKey
                var serviceSectionKey = serviceType.FullName;
                if (string.IsNullOrEmpty(serviceSectionKey) == true) throw new InvalidOperationException($"{nameof(serviceSectionKey)}=null");

                // ServiceSectionValue
                var serviceSectionValue = configuration.GetSection(serviceSectionKey);
                if (serviceSectionValue == null) throw new InvalidOperationException($"{nameof(serviceSectionValue)}=null");

                // Attach
                _serviceSection = serviceSectionValue;
            }
        }


        // Properties
        public IConfiguration RootSection { get { return _rootSection; } }

        public IConfiguration ServiceSection { get { return _serviceSection; } }


        // Methods
        public TValue GetValue<TValue>(string valueKey)
           where TValue : notnull
        {
            #region Contracts

            if (string.IsNullOrEmpty(valueKey) == true) throw new ArgumentException(nameof(valueKey));

            #endregion

            // Result
            string valueString = null;

            // ServiceValueString
            var serviceValueString = this.GetServiceValueString(valueKey);
            if (string.IsNullOrEmpty(serviceValueString) == false) valueString = serviceValueString;
            if (string.IsNullOrEmpty(serviceValueString) == true) return default(TValue);

            // GlobalValueString
            var globalValueString = this.GetGlobalValueString(serviceValueString);
            if (string.IsNullOrEmpty(globalValueString) == false) valueString = globalValueString;

            // ValueString
            if (string.IsNullOrEmpty(valueString) == true)
            {
                return default(TValue);
            }
            return (TValue)Convert.ChangeType(valueString, typeof(TValue));
        }

        private string GetServiceValueString(string valueKey)
        {
            #region Contracts

            if (string.IsNullOrEmpty(valueKey) == true) throw new ArgumentException(nameof(valueKey));

            #endregion

            // ServiceValueString
            var serviceValueString = this.ServiceSection?.GetValue<string>(valueKey);
            if (string.IsNullOrEmpty(serviceValueString) == true) return null;

            // Return
            return serviceValueString;
        }

        private string GetGlobalValueString(string valueString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(valueString) == true) throw new ArgumentException(nameof(valueString));

            #endregion

            // GlobalSectionKey
            var globalSectionKey = Regex.Match(valueString, @"(?<=^\[).+?(?=\])").Value;
            if (string.IsNullOrEmpty(globalSectionKey) == true) return null;

            // GlobalSection
            var globalSection = this.RootSection?.GetSection(globalSectionKey);
            if (globalSection == null) return null;

            // GlobalValueKey
            var globalValueKey = valueString.Remove(0, globalSectionKey.Length + 2);
            if (string.IsNullOrEmpty(globalValueKey) == true) return null;

            // GlobalValueString
            var globalValueString = globalSection.GetValue<string>(globalValueKey);
            if (string.IsNullOrEmpty(globalValueString) == true) return null;

            // Return
            return globalValueString;
        }
    }
}
