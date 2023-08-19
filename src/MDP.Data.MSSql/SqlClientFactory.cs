using System;
using System.Collections.Generic;

namespace MDP.Data.MSSql
{
    [MDP.Registration.Service<SqlClientFactory>(singleton: true)]
    public class SqlClientFactory
    {
        // Fields
        private readonly Dictionary<string, string> _connectionStrings;


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
        public SqlClient CreateClient(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // ConnectionString
            var connectionString = string.Empty;
            if (_connectionStrings.ContainsKey(name) == true) connectionString = _connectionStrings[name];
            if (string.IsNullOrEmpty(connectionString) == true) throw new InvalidOperationException($"{nameof(connectionString)}=null: name={name}");

            // Return
            return new SqlClient(connectionString);
        }
    }
}
