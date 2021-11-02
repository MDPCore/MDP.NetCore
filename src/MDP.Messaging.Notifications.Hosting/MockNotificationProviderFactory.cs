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
    public class MockNotificationProviderFactory : Factory<NotificationProvider, MockNotificationProvider>
    {
        // Methods
        protected override MockNotificationProvider CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            
            #endregion

            // Create
            return new MockNotificationProvider();
        }
    }
}
