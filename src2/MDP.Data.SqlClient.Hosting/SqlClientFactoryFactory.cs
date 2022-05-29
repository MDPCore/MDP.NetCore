using Autofac;
using MDP.Data.SqlClient;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlClientFactoryFactory : ServiceFactory<SqlClientFactory, SqlClientFactory, SqlClientFactoryFactory.Setting>
    {
        // Constructors
        public SqlClientFactoryFactory() 
        {
            // Default
            this.ServiceSingleton = true;
        }


        // Methods
        protected override SqlClientFactory CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

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
            public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
        }
    }
}
