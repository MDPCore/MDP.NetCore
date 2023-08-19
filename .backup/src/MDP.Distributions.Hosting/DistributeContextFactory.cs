using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Hosting
{
    public class DistributeContextFactory : ServiceFactory<DistributeContext, DistributeContext, DistributeContextFactory.Setting>
    {
        // Constructors
        public DistributeContextFactory()
        {
            // Default
            this.ServiceSingleton = true;
        }


        // Methods
        protected override DistributeContext CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new DistributeContext
            (
                componentContext.Resolve<ResourceLockRepository>(setting.ResourceLockRepository)
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string ResourceLockRepository { get; set; } = String.Empty;
        }
    }
}
