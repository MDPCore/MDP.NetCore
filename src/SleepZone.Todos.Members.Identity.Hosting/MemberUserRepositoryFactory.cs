using Autofac;
using MDP.Hosting;
using MDP.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Members.Identity.Hosting
{
    public class MemberUserRepositoryFactory : Factory<UserRepository, MemberUserRepository, MemberUserRepositoryFactory.Setting>
    {
        // Methods
        protected override MemberUserRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MemberUserRepository
            (
                componentContext.Resolve<MemberContext>()
            );
        }


        // Class
        public class Setting
        {

        }
    }
}
