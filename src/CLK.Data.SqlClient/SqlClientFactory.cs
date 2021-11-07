using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Data.SqlClient
{
    public class SqlClientFactory
    {
        // Fields
        private readonly Dictionary<string, string> _connectionStrings = null;


        // Constructors
        public SqlClientFactory(Dictionary<string, string> connectionStrings)
        {
            #region Contracts

            if (connectionStrings == null) throw new ArgumentException(nameof(connectionStrings));

            #endregion

            // Default
            _connectionStrings = new Dictionary<string, string>(connectionStrings, StringComparer.OrdinalIgnoreCase);
        }


        // Methods
        public SqlCommandEntity CreateCommand(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // ConnectionString
            var connectionString = string.Empty;
            if (_connectionStrings.ContainsKey(name) == true) connectionString = _connectionStrings[name];
            if (string.IsNullOrEmpty(connectionString) == true) throw new InvalidOperationException($"{nameof(connectionString)}=null: name={name}");

            // CommandEntity
            var commandEntity = this.CreateCommandEntity(connectionString);
            if (commandEntity == null) throw new InvalidOperationException($"{nameof(commandEntity)}=null: name={name}");

            // Return
            return commandEntity; 
        }

        protected virtual SqlCommandEntity CreateCommandEntity(string connectionString)
        {
            #region Contracts

            if (connectionString == null) throw new ArgumentException(nameof(connectionString));

            #endregion

            // Create
            return new SqlCommandEntity(connectionString);
        }
    }
}
