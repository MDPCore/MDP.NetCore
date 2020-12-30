using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDP.Data.SqlClient
{
    public sealed class SqlCommandScope : IDisposable
    {
        // Fields
        private readonly SqlConnection _connection = null;

        private readonly SqlCommand _command = null;


        // Constructors
        public SqlCommandScope(string connectionString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(connectionString) == true) throw new ArgumentNullException();

            #endregion

            // Connection
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            // Command
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        public void Dispose()
        {
            // Exception
            Exception exception = null;

            // Command
            if (_command != null)
            {
                try
                {
                    _command.Dispose();
                }
                catch (Exception ex)
                {
                    if (exception == null) exception = ex;
                }
            }

            // Connection
            if (_connection != null)
            {
                try
                {
                    _connection.Dispose();
                }
                catch (Exception ex)
                {
                    if (exception == null) exception = ex;
                }
            }

            // Throw
            if (exception != null) throw exception;
        }


        // Properties
        public SqlParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }

        public string CommandText
        {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
        }

        public int CommandTimeout
        {
            get { return _command.CommandTimeout; }
            set { _command.CommandTimeout = value; }
        }


        // Methods
        public int ExecuteNonQuery()
        {
            // Clean
            this.CleanParameters();

            // Execute
            return _command.ExecuteNonQuery();
        }

        public object ExecuteScalar()
        {
            // Clean
            this.CleanParameters();

            // Execute
            return _command.ExecuteScalar();
        }

        public SqlDataReader ExecuteReader()
        {
            // Clean
            this.CleanParameters();

            // Execute
            return _command.ExecuteReader();
        }


        private void CleanParameters()
        {
            // Parameters
            foreach (var parameter in this.Parameters.OfType<SqlParameter>())
            {
                // DBNull
                if (parameter.Value == null)
                {
                    // Setting
                    parameter.Value = DBNull.Value;

                    // Continue
                    continue;
                }

                // DateTime.MinValue
                if (parameter.Value is DateTime && (DateTime)(parameter.Value) == DateTime.MinValue)
                {
                    // Setting
                    parameter.Value = new DateTime(1753, 1, 1);

                    // Continue
                    continue;
                }
            }
        }
    }
}
