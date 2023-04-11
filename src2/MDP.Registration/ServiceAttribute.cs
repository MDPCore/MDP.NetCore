using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ServiceAttribute : Attribute
    {
        // Constructors
        internal ServiceAttribute()
        {

        }


        // Properties
        public abstract string ServiceNamespace { get; }

        public abstract bool ServiceSingleton { get; }

        public abstract Type ServiceType { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute<TService> : ServiceAttribute
        where TService : class
    {
        // Fields
        private string? _serviceNamespace = String.Empty;

        private bool _serviceSingleton = false;


        // Constructors
        public ServiceAttribute(string? @namespace = null, bool @singleton = false)
        {
            // Default
            _serviceNamespace = @namespace;
            _serviceSingleton = @singleton;
        }


        // Properties
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

        public override bool ServiceSingleton { get { return _serviceSingleton; } }

        public override Type ServiceType { get { return typeof(TService); } }
    }
}
