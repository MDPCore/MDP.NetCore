using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class FactoryAttribute : Attribute
    {
        // Constructors
        internal FactoryAttribute()
        {

        }


        // Properties
        public abstract string ServiceNamespace { get; }

        public abstract string? ServiceName { get; }

        public abstract Type BuilderType { get; }

        public abstract Type SettingType { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FactoryAttribute<TBuilder, TSetting> : FactoryAttribute
        where TBuilder : class
        where TSetting : class, new()
    {
        // Fields
        private string _serviceNamespace = String.Empty;

        private string? _serviceName = String.Empty;


        // Constructors
        public FactoryAttribute(string @namespace, string? @name = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(@namespace) == true) throw new ArgumentException($"{nameof(@namespace)}=null");

            #endregion

            // Default
            _serviceNamespace = @namespace;
            _serviceName = @name;
        }


        // Properties
        public override string ServiceNamespace { get { return _serviceNamespace; } }

        public override string? ServiceName { get { return _serviceName; } }

        public override Type BuilderType { get { return typeof(TBuilder); } }

        public override Type SettingType { get { return typeof(TSetting); } }
    }
}
