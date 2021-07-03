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
    public class SqlClientFactoryFactory : Factory<SqlClientFactory, SqlClientFactoryOptions>
    {
        // Constructors
        public SqlClientFactoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override SqlClientFactory Create(IComponentContext componentContext, SqlClientFactoryOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Create
            var context = new SqlClientFactory
            (
                options.ConnectionStrings
            );

            // Return
            return context;
        }
    }
}
