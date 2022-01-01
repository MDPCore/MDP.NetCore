using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Mocks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class MockRegistrationRepositoryFactory : Factory<RegistrationRepository, MockRegistrationRepository, MockRegistrationRepositoryFactory.Setting>
    {
        // Methods
        protected override MockRegistrationRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Repository
            var repository = new MockRegistrationRepository();

            // RegistrationList
            foreach (var registration in setting.RegistrationList)
            {
                // Add
                repository.Add(new Registration()
                {
                    UserId = registration.UserId,
                    DeviceType = registration.DeviceType,
                    Token = registration.Token
                });
            }

            // Return
            return repository;
        }


        // Class
        public class Setting
        {
            // Properties
            public List<Registration> RegistrationList { get; set; } = new List<Registration>();
        }
    }
}
