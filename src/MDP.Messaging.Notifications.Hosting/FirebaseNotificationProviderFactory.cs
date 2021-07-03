using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Firebase;

namespace MDP.Messaging.Notifications.Hosting
{
    public class FirebaseNotificationProviderFactory : Factory<FirebaseNotificationProvider>
    {
        // Constructors
        public FirebaseNotificationProviderFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override FirebaseNotificationProvider Create(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Return
            return componentContext.Resolve<FirebaseNotificationProvider>();
        }
    }
}
