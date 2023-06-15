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
        public abstract Type ServiceType { get; }

        public abstract bool Singleton { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute<TService> : ServiceAttribute
        where TService : class
    {
        // Fields
        private bool _singleton = false;


        // Constructors
        public ServiceAttribute(bool singleton = false)
        {
            // Default
            _singleton = singleton;
        }


        // Properties
        public override Type ServiceType { get { return typeof(TService); } }

        public override bool Singleton { get { return _singleton; } }
    }
}
