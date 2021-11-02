using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public interface Factory<TService>
        where TService : class
    {
        // Methods
        TService Create(IComponentContext componentContext);
    }

    public abstract class Factory<TService, TInstance> : Factory<TService>
        where TService : class
        where TInstance : TService
    {
        // Methods
        public TService Create(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // InitializeFactory
            if (this.InitializeFactory(componentContext) == false) return null;

            // CreateService
            return this.CreateService(componentContext);
        }        

        protected virtual bool InitializeFactory(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Initialize
            if (this.BindFactory(componentContext) == false) return false;

            // Return
            return true;
        }

        protected abstract TInstance CreateService(IComponentContext componentContext);


        private bool BindFactory(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // ConfigRoot
            var configRoot = componentContext.Resolve<IConfiguration>();
            if (configRoot == null) throw new InvalidOperationException($"{nameof(configRoot)}=null");

            // BindPath
            var bindPath = this.CreateBindPath();
            if (string.IsNullOrEmpty(bindPath) == true) return false;

            // BindSection
            var bindSection = configRoot.GetSection(bindPath);
            if (bindSection == null) return false;
            if (bindSection.Exists() == false) return false;

            // Bind
            ConfigurationBinder.Bind(bindSection, this);

            // Return
            return true;
        }

        protected virtual string CreateBindPath()
        {
            // BindPath
            var bindPath = typeof(TService).Namespace + ":" + this.GetType().Name;

            // Suffix
            if (bindPath.EndsWith("Factory")==true)
            {
                bindPath = bindPath.Substring(0, bindPath.Length - "Factory".Length);
            }

            // Return
            return bindPath;
        }
    }
}
