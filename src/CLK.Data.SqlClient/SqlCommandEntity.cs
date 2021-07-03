using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace CLK.Data.SqlClient
{
    public class SqlCommandEntity : IDisposable
    {
        // Fields
        private readonly SqlConnection _connection = null;

        private readonly SqlCommand _command = null;


        // Constructors
        public SqlCommandEntity(string connectionString)
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

        public virtual void Dispose()
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
        public virtual string CommandText
        {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
        }

        public virtual int CommandTimeout
        {
            get { return _command.CommandTimeout; }
            set { _command.CommandTimeout = value; }
        }


        // Methods
        public virtual void AddParameter(string name, object value, SqlDbType sqlDbType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentNullException();

            #endregion

            // Add
            _command.Parameters.Add(new SqlParameter(name, sqlDbType)).Value = value;
        }

        public virtual void AddParameter(string name, object value, SqlDbType sqlDbType, int size)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentNullException();

            #endregion

            // Add
            _command.Parameters.Add(new SqlParameter(name, sqlDbType, size)).Value = value;
        }

        public virtual void ClearParameters()
        {
            // Clear
            _command.Parameters.Clear();
        }


        public virtual int ExecuteNonQuery()
        {
            // Execute
            return this.Execute(() => _command.ExecuteNonQuery());
        }

        public virtual object ExecuteScalar()
        {
            // Execute
            return this.Execute(() => _command.ExecuteScalar());
        }

        public virtual SqlDataReader ExecuteReader()
        {
            // Execute
            return this.Execute(() => _command.ExecuteReader());
        }


        private T Execute<T>(Func<T> action)
        {
            #region Contracts

            if (action == null) throw new ArgumentNullException($"{nameof(action)}");

            #endregion

            // Replace
            this.ReplaceParameters();

            // Execute
            try
            {
                // Invoke
                return action();
            }
            catch (SqlException ex)
            {
                // DuplicateKeyException
                if (ex.Errors[0].Number == 2627) throw new DuplicateKeyException();

                // Throw
                throw;
            }
        }

        private void ReplaceParameters()
        {
            // Parameters
            foreach (var parameter in _command.Parameters.OfType<SqlParameter>())
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
