using Autofac;
using Autofac.Core.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting.Lab
{
    public class HelloWorkServiceModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // WorkService
            containerBuilder.RegisterService<WorkService>();
            containerBuilder.RegisterFactory<WorkService, HelloWorkServiceFactory>();
        }
    }
}
