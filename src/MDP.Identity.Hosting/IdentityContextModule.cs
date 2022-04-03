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
            container.RegisterService<IdentityContext>().SingleInstance();
            container.RegisterFactory<IdentityContext, IdentityContextFactory>();

            // RoleRepository
            container.RegisterService<RoleRepository>();
            container.RegisterFactory<RoleRepository, MockRoleRepositoryFactory>();

            // UserRepository
            container.RegisterService<UserRepository>();
            container.RegisterFactory<UserRepository, MockUserRepositoryFactory>();

            // UserRoleRepository
            container.RegisterService<UserRoleRepository>();
            container.RegisterFactory<UserRoleRepository, MockUserRoleRepositoryFactory>();

            // UserLoginRepository
            container.RegisterService<UserLoginRepository>();
            container.RegisterFactory<UserLoginRepository, MockUserLoginRepositoryFactory>();

            // UserTokenRepository
            container.RegisterService<UserTokenRepository>();
            container.RegisterFactory<UserTokenRepository, MockUserTokenRepositoryFactory>();
        }
    }
}
