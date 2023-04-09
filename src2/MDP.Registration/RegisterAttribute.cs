using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public abstract class RegisterAttribute : Attribute
    {
        // Properties
        public abstract Type ServiceType { get; }

        public abstract string ServiceNamespace { get; }

        public abstract bool ServiceSingleton { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterAttribute<TService> : RegisterAttribute
    {
        // Fields
        private string? _serviceNamespace = String.Empty;

        private bool _serviceSingleton = false;


        // Constructors
        public RegisterAttribute(string? serviceNamespace = null, bool serviceSingleton = false)
        {
            // Default
            _serviceNamespace = serviceNamespace;
            _serviceSingleton = serviceSingleton;
        }


        // Properties
        public override Type ServiceType
        {
            get { return typeof(TService); }
        }

        public override string ServiceNamespace
        {
            get 
            {
                // ServiceNamespace
                var serviceNamespace = _serviceNamespace;
                if (string.IsNullOrEmpty(serviceNamespace) == true) serviceNamespace = this.ServiceType.Namespace;
                if (string.IsNullOrEmpty(serviceNamespace) == true) throw new InvalidOperationException($"{nameof(serviceNamespace)}=null");

                // Return
                return serviceNamespace;
            }
        }

        public override bool ServiceSingleton
        {
            get { return _serviceSingleton; }
        }
    }
}
