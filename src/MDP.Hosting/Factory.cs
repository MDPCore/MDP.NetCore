using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Factory<TService>
        where TService : class
    {
        // Constructors
        internal Factory() { }

        // Methods
        internal abstract TService Create(IComponentContext componentContext, IConfiguration serviceConfig);
    }

    public abstract class Factory<TService, TInstance, TSetting> : Factory<TService>
        where TService : class
        where TInstance : TService
        where TSetting : class, new()
    {
        // Methods
        internal override TService Create(IComponentContext componentContext, IConfiguration serviceConfig)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (serviceConfig == null) throw new ArgumentException(nameof(serviceConfig));

            #endregion

            // ServiceSetting
            var serviceSetting = new TSetting();
            ConfigurationBinder.Bind(serviceConfig, serviceSetting);

            // CreateService
            return this.CreateService(componentContext, serviceSetting);
        }

        protected abstract TInstance CreateService(IComponentContext componentContext, TSetting setting);
    }
}