using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public abstract class Factory
    {
        // Constructors
        internal Factory(string @namespace, string @service = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(@namespace) == true) throw new ArgumentException($"{nameof(@namespace)}=null");

            #endregion

            // Default
            this.ServiceNamespace = @namespace;
            this.ServiceName = @service;
        }


        // Properties
        public string ServiceNamespace { get; }

        public string ServiceName { get; }
    }

    public abstract class Factory<TBuilder, TSetting> : Factory
        where TBuilder : class
        where TSetting : class, new()
    {
        // Constructors
        public Factory(string @namespace, string @service = null) : base(@namespace, @service) { }


        // Methods
        public abstract void ConfigureService(TBuilder builder, TSetting setting);
    }
}
