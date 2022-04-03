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
    public class MockUserTokenRepositoryFactory : Factory<UserTokenRepository, MockUserTokenRepository, MockUserTokenRepositoryFactory.Setting>
    {
        // Methods
        protected override MockUserTokenRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockUserTokenRepository();
        }


        // Class
        public class Setting
        {

        }
    }
}
