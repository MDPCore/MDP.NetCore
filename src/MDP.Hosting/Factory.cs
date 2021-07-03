using Autofac;
using CLK.Autofac;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Factory<TService>
        where TService : class
    {
        // Fields
        private readonly IComponentContext _componentContext = null;


        // Constructors
        public Factory(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Default
            _componentContext = componentContext;
        }


        // Methods
        public TService Create()
        {
            // Return
            return this.Create(_componentContext);
        }

        protected abstract TService Create(IComponentContext componentContext);
    }

    public abstract class Factory<TService, TOptions>
        where TService : class 
        where TOptions : class
    {
        // Fields
        private readonly IComponentContext _componentContext = null;

        private readonly IOptionsMonitor<TOptions> _optionsMonitor = null;


        // Constructors
        public Factory(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Default
            _componentContext = componentContext;
            _optionsMonitor = componentContext.ResolveRequired<IOptionsMonitor<TOptions>>();
        }


        // Methods
        public TService Create(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // Options
            var options = _optionsMonitor.Get(name);
            if (options == null) throw new InvalidOperationException($"{nameof(options)}=null");

            // Return
            return this.Create(_componentContext, options);
        }

        protected abstract TService Create(IComponentContext componentContext, TOptions options);
    }
}
