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

        public abstract bool AutoRegister { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute<TService> : ServiceAttribute
        where TService : class
    {
        // Fields
        private bool _singleton = false;

        private bool _autoRegister = true;


        // Constructors
        public ServiceAttribute(bool singleton = false, bool autoRegister = true)
        {
            // Default
            _singleton = singleton;
            _autoRegister = autoRegister;
        }


        // Properties
        public override Type ServiceType { get { return typeof(TService); } }

        public override bool Singleton { get { return _singleton; } }

        public override bool AutoRegister { get { return _autoRegister; } }
    }
}
