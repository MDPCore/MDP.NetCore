using Autofac;
using MDP;
using MDP.Hosting;
using MDP.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace SleepZone.Todos.Members.Identity.Hosting
{
    public class IdentityContextModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // UserRepository
            container.RegisterFactory<UserRepository, MemberUserRepositoryFactory>(this.Configuration);
        }
    }
}
