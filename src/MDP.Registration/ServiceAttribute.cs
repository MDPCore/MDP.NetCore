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
        public abstract bool ServiceSingleton { get; }

        public abstract Type ServiceType { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute<TService> : ServiceAttribute
        where TService : class
    {
        // Fields
        private bool _serviceSingleton = false;


        // Constructors
        public ServiceAttribute(bool singleton = false)
        {
            // Default
            _serviceSingleton = singleton;
        }


        // Properties
        public override bool ServiceSingleton { get { return _serviceSingleton; } }

        public override Type ServiceType { get { return typeof(TService); } }
    }
}
