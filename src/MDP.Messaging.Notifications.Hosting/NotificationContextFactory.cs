using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextFactory : Factory<NotificationContext, NotificationContext>
    {
        // Properties
        public string RegistrationRepository { get; set; }

        public string NotificationProvider { get; set; }


        // Methods
        protected override NotificationContext CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
          
            #endregion

            // Create
            return new NotificationContext
            (
                componentContext.ResolveNamed<RegistrationRepository>(this.RegistrationRepository),
                componentContext.ResolveNamed<NotificationProvider>(this.NotificationProvider)
            );
        }
    }
}
