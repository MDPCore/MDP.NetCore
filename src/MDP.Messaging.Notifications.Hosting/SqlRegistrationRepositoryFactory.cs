using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Accesses;

namespace MDP.Messaging.Notifications.Hosting
{
    public class SqlRegistrationRepositoryFactory : Factory<RegistrationRepository, SqlRegistrationRepository, SqlRegistrationRepositoryFactory.Setting>
    {
        // Methods
        protected override SqlRegistrationRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new SqlRegistrationRepository
            (
                componentContext.Resolve<SqlClientFactory>()
            );
        }


        // Class
        public class Setting
        {

        }
    }
}
