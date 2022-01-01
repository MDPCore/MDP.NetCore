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
    public class MockUserLoginRepositoryFactory : Factory<UserLoginRepository, MockUserLoginRepository, MockUserLoginRepositoryFactory.Setting>
    {
        // Methods
        protected override MockUserLoginRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockUserLoginRepository();
        }


        // Class
        public class Setting
        {

        }
    }
}
