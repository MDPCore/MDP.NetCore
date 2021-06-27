using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;

namespace MDP.Data.SqlClient.Hosting
{
    public class DefaultSqlCommandFactory : Factory<SqlCommandScope, SqlCommandOption>, SqlCommandFactory
    {
        // Constructors
        public DefaultSqlCommandFactory(IOptionsMonitor<SqlCommandOption> optionsMonitor): base(optionsMonitor)
        {

        }


        // Methods
        protected override SqlCommandScope Create(SqlCommandOption options)
        {
            #region Contracts

            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Create
            var command = new SqlCommandScope(options.ConnectionString);

            // Return
            return command;
        }
    }
}
