using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Factory<TComponent, TOptions> where TComponent : class where TOptions : class
    {
        // Fields
        private readonly IOptionsMonitor<TOptions> _optionsMonitor = null;


        // Constructors
        public Factory(IOptionsMonitor<TOptions> optionsMonitor)
        {
            #region Contracts

            if (optionsMonitor == null) throw new ArgumentException(nameof(optionsMonitor));

            #endregion

            // Default
            _optionsMonitor = optionsMonitor;
        }


        // Methods
        public TComponent Create()
        {
            // Create
            return this.Create(Options.DefaultName);
        }

        public TComponent Create(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // Options
            var options = _optionsMonitor.Get(name);
            if (options == null) throw new InvalidOperationException($"{nameof(options)}=null");

            // Return
            return this.Create(options);
        }

        protected abstract TComponent Create(TOptions options);
    }
}
