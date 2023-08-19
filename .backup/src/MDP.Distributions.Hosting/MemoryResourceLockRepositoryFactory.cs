using Autofac;
using MDP.Hosting;
using MDP.Distributions.Accesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Hosting
{
    public class MemoryResourceLockRepositoryFactory : ServiceFactory<ResourceLockRepository, MemoryResourceLockRepository, MemoryResourceLockRepositoryFactory.Setting>
    {
        // Methods
        protected override MemoryResourceLockRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MemoryResourceLockRepository();
        }


        // Class
        public class Setting
        {

        }
    }
}
