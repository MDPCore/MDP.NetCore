using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextFactory : Factory<NotificationContext, NotificationContextOptions>
    {
        // Constructors
        public NotificationContextFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override NotificationContext Create(IComponentContext componentContext, NotificationContextOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Create
            var context = new NotificationContext
            (
                componentContext.ResolveNamed<RegistrationRepository>(options.RegistrationRepository),
                componentContext.ResolveNamed<NotificationProvider>(options.NotificationProvider)
            );

            // Return
            return context;
        }
    }
}
