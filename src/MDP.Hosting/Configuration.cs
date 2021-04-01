using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    public class Configuration<TService> : IConfiguration<TService>
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
                var serviceSectionKey = $"{serviceType.Namespace}:{serviceType.Name}";
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
    }
}
