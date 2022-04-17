using Autofac;
using MDP;
using MDP.Hosting;
using Microsoft.Extensions.Configuration;
using MDP.Identity.Mocks;
using System;

namespace MDP.Identity.Hosting
{
    public class IdentityContextModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // TodoContext
            container.RegisterFactory<IdentityContext, IdentityContextFactory>(this.Configuration).SingleInstance(); 

            // RoleRepository
            container.RegisterFactory<RoleRepository, MockRoleRepositoryFactory>(this.Configuration);

            // UserRepository
            container.RegisterFactory<UserRepository, MockUserRepositoryFactory>(this.Configuration);

            // UserRoleRepository
            container.RegisterFactory<UserRoleRepository, MockUserRoleRepositoryFactory>(this.Configuration);

            // UserLoginRepository
            container.RegisterFactory<UserLoginRepository, MockUserLoginRepositoryFactory>(this.Configuration);

            // UserTokenRepository
            container.RegisterFactory<UserTokenRepository, MockUserTokenRepositoryFactory>(this.Configuration);
        }
    }
}
