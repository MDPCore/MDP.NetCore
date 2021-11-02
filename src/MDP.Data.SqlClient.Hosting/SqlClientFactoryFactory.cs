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
    public class SqlClientFactoryFactory : Factory<SqlClientFactory, SqlClientFactory>
    {
        // Properties
        public Dictionary<string, string> ConnectionStrings { get; set; }


        // Methods
        protected override SqlClientFactory CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Create
            return new SqlClientFactory
            (
                this.ConnectionStrings
            );
        }
    }
}
