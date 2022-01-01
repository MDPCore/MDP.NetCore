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
    public class MockUserRoleRepositoryFactory : Factory<UserRoleRepository, MockUserRoleRepository, MockUserRoleRepositoryFactory.Setting>
    {
        // Methods
        protected override MockUserRoleRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockUserRoleRepository();
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; } = "Hello World";
        }
    }
}
