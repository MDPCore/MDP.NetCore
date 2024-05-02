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
        public SqlClient CreateClient(string name = null)
        {
            // SqlClientBuilder
            SqlClientBuilder builder = null;
            if (string.IsNullOrEmpty(name) == true)
            {
                builder = _builderDictionary.Values.FirstOrDefault();
            }
            if (string.IsNullOrEmpty(name) == false && _builderDictionary.ContainsKey(name) == true)
            {
                builder = _builderDictionary[name];
            }
            if (builder == null) return null;

            // SqlClient
            var sqlClient = builder.CreateClient();
            if (sqlClient == null) throw new InvalidOperationException($"{nameof(sqlClient)}=null");

            // Return
            return sqlClient;
        }
    }
}
