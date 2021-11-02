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
    public class MockUserRoleRepositoryFactory : Factory<UserRoleRepository, MockUserRoleRepository>
    {
        // Methods
        protected override MockUserRoleRepository CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            
            #endregion

            // Create
            return new MockUserRoleRepository();
        }
    }
}
