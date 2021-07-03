using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Mocks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class MockRegistrationRepositoryFactory : Factory<MockRegistrationRepository, MockRegistrationRepositoryOptions>
    {
        // Constructors
        public MockRegistrationRepositoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override MockRegistrationRepository Create(IComponentContext componentContext, MockRegistrationRepositoryOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Repository
            var repository = new MockRegistrationRepository();

            // RegistrationList
            foreach (var registrationOptions in options.RegistrationList)
            {
                // Add
                repository.Add(new Registration()
                {
                    UserId = registrationOptions.UserId,
                    DeviceType = registrationOptions.DeviceType,
                    Token = registrationOptions.Token
                });
            }

            // Return
            return repository;
        }
    }
}
