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
    public class MockNotificationProviderFactory : Factory<NotificationProvider, MockNotificationProvider, MockNotificationProviderFactory.Setting>
    {
        // Methods
        protected override MockNotificationProvider CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockNotificationProvider();
        }


        // Class
        public class Setting
        {

        }
    }
}
