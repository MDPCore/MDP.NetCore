using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextFactory : Factory<NotificationContext, NotificationContext, NotificationContextFactory.Setting>
    {
        // Methods
        protected override NotificationContext CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new NotificationContext
            (
                componentContext.Resolve<RegistrationRepository>(setting.RegistrationRepository),
                componentContext.Resolve<NotificationProvider>(setting.NotificationProvider)
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string RegistrationRepository { get; set; }

            public string NotificationProvider { get; set; }
        }
    }
}
