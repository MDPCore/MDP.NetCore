using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public abstract class ServiceFactory
    {
        // Constructors
        internal ServiceFactory(string @namespace, string @service = null, bool @autoRegister = true)
        {
            #region Contracts

            if (string.IsNullOrEmpty(@namespace) == true) throw new ArgumentException($"{nameof(@namespace)}=null");

            #endregion

            // Default
            this.ServiceNamespace = @namespace;
            this.ServiceName = @service;
            this.AutoRegister = @autoRegister;
        }


        // Properties
        public string ServiceNamespace { get; }

        public string ServiceName { get; }

        public bool AutoRegister { get; }
    }

    public abstract class ServiceFactory<TBuilder> : ServiceFactory
        where TBuilder : class
    {
        // Constructors
        public ServiceFactory(string @namespace, string @service = null, bool @autoRegister = true) : base(@namespace, @service, @autoRegister) { }


        // Methods
        public abstract void ConfigureService(TBuilder builder);
    }

    public abstract class ServiceFactory<TBuilder, TSetting> : ServiceFactory
        where TBuilder : class
        where TSetting : class, new()
    {
        // Constructors
        public ServiceFactory(string @namespace, string @service = null, bool @autoRegister = true) : base(@namespace, @service, @autoRegister) { }


        // Methods
        public abstract void ConfigureService(TBuilder builder, TSetting setting);
    }
}
