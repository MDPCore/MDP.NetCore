using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using MDP.Identity.Mocks;

namespace MDP.Identity.Hosting
{
    public class MockRoleRepositoryFactory : Factory<RoleRepository, MockRoleRepository, MockRoleRepositoryFactory.Setting>
    {
        // Methods
        protected override MockRoleRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockRoleRepository();
        }


        // Class
        public class Setting
        {

        }
    }
}
