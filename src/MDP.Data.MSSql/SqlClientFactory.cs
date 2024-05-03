using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Data.MSSql
{
    public class SqlClientFactory
    {
        // Fields
        private readonly Dictionary<string, SqlClientBuilder> _builderDictionary;


        // Constructors
        public SqlClientFactory(IList<SqlClientBuilder> builderList)
        {
            #region Contracts

            if (builderList == null) throw new ArgumentException(nameof(builderList));

            #endregion

            // Default
            _builderDictionary = builderList.ToDictionary(o => o.Name, o => o, StringComparer.OrdinalIgnoreCase);
        }


        // Methods
        public SqlClient CreateClient(string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // SqlClientBuilder
            SqlClientBuilder builder = null;
            if (_builderDictionary.ContainsKey(name) == true) builder = _builderDictionary[name];
            if (builder == null) throw new InvalidOperationException($"{nameof(builder)}=null");

            // SqlClient
            var sqlClient = builder.CreateClient();
            if (sqlClient == null) throw new InvalidOperationException($"{nameof(sqlClient)}=null");

            // Return
            return sqlClient;
        }
    }
}
