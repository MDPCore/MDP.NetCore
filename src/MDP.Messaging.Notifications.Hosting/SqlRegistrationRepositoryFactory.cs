using CLK.Data.SqlClient;
using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using MDP.Messaging.Notifications.Accesses;

namespace MDP.Messaging.Notifications.Hosting
{
    public class SqlRegistrationRepositoryFactory : Factory<SqlRegistrationRepository>
    {
        // Constructors
        public SqlRegistrationRepositoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override SqlRegistrationRepository Create(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Return
            return componentContext.Resolve<SqlRegistrationRepository>();
        }
    }
}
