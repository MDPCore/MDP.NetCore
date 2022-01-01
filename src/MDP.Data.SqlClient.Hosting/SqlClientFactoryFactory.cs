using Autofac;
using CLK.Data.SqlClient;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlClientFactoryFactory : Factory<SqlClientFactory, SqlClientFactory, SqlClientFactoryFactory.Setting>
    {
        // Methods
        protected override SqlClientFactory CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new SqlClientFactory
            (
                setting.ConnectionStrings
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public Dictionary<string, string> ConnectionStrings { get; set; }
        }
    }
}
