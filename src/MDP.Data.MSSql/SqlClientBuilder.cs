using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.MSSql
{
    public class SqlClientBuilder
    {
        // Constructors
        public SqlClientBuilder(string name, string connectionString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");
            if (string.IsNullOrEmpty(connectionString) == true) throw new ArgumentException($"{nameof(connectionString)}=null");
          
            #endregion

            // Default
            this.Name = name;
            this.ConnectionString = connectionString;
            this.Handlers = new List<SqlClientHandler>();
        }


        // Properties
        public string Name { get;  } = string.Empty;

        public string ConnectionString { get; } = string.Empty;

        public List<SqlClientHandler> Handlers { get; } = null;


        // Methods
        public SqlClient CreateClient(string name)
        {
            #region Contracts

            if (name == null) throw new ArgumentException(nameof(name));

            #endregion

            // SqlClient
            var sqlClient = new SqlClient(this.ConnectionString);

            // SqlClientHandler
            foreach (var sqlClientHandler in this.Handlers)
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
