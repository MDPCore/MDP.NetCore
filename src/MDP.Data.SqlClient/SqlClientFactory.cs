using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.SqlClient
{
    public class SqlClientFactory
    {
        // Fields
        private readonly SqlCommandFactory _commandFactory = null;


        // Constructors
        public SqlClientFactory(SqlCommandFactory commandFactory)
        {
            #region Contracts

            if (commandFactory == null) throw new ArgumentException(nameof(commandFactory));

            #endregion

            // Default
            _commandFactory = commandFactory;
        }


        // Methods
        public SqlCommandScope CreateCommand()
        {
            // Create
            return _commandFactory.Create();
        }

        public SqlCommandScope CreateCommand(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // Create
            return _commandFactory.Create(name);
        }
    }
}
