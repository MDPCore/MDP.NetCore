using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Data.MSSql
{
    public class SqlClientFactory
    {
        // Fields
        private readonly Dictionary<string, SqlClientOptions> _optionsDictionary;


        // Constructors
        public SqlClientFactory(IList<SqlClientOptions> optionsList)
        {
            #region Contracts

            if (optionsList == null) throw new ArgumentException(nameof(optionsList));

            #endregion

            // Default
            _optionsDictionary = optionsList.ToDictionary(o => o.Name, o => o, StringComparer.OrdinalIgnoreCase);
        }


        // Methods
        public SqlClient CreateClient(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // SqlClientOptions
            SqlClientOptions options = null;
            if (_optionsDictionary.ContainsKey(name) == true) options = _optionsDictionary[name];
            if (options == null) throw new InvalidOperationException($"{nameof(options)}=null: name={name}");

            // SqlClient
            var sqlClient = new SqlClient(options.ConnectionString);
            
            // SqlClientHandler
            foreach (var sqlClientHandler in options.Handlers)
            {
                // Handle
                sqlClientHandler.Handle(sqlClient);
            }

            // Open
            sqlClient.Connection.Open();

            // Return
            return sqlClient;
        }
    }
}
