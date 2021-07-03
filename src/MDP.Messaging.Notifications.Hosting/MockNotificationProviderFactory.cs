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
    public class MockNotificationProviderFactory : Factory<MockNotificationProvider, MockNotificationProviderOptions>
    {
        // Constructors
        public MockNotificationProviderFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override MockNotificationProvider Create(IComponentContext componentContext, MockNotificationProviderOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Repository
            var repository = new MockNotificationProvider();

            // Return
            return repository;
        }
    }
}
