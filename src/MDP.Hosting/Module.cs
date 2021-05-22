using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class Module : Autofac.Module
    {
        // Methods
        protected override void Load(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // Base
            base.Load(container);

            // Configure
            this.ConfigureContainer(container);
        }

        protected abstract void ConfigureContainer(ContainerBuilder container);
    }
}
