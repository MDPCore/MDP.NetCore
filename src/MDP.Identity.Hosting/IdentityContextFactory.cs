using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Hosting
{
    public class IdentityContextFactory : Factory<IdentityContext, IdentityContext, IdentityContextFactory.Setting>
    {
        // Methods
        protected override IdentityContext CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new IdentityContext
            (
                componentContext.Resolve<RoleRepository>(setting.RoleRepository),
                componentContext.Resolve<UserRepository>(setting.UserRepository),
                componentContext.Resolve<UserRoleRepository>(setting.UserRoleRepository),
                componentContext.Resolve<UserLoginRepository>(setting.UserLoginRepository),
                componentContext.Resolve<UserTokenRepository>(setting.UserTokenRepository),
                componentContext.ResolveAll<IdentityService>()
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string RoleRepository { get; set; }

            public string UserRepository { get; set; }

            public string UserRoleRepository { get; set; }

            public string UserLoginRepository { get; set; }

            public string UserTokenRepository { get; set; }
        }
    }
}
