using MDP.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SleepZone.Todos.Accesses
{
    [MDP.Registration.Service<TodoRepository>()]
    public class SqlTodoRepository : TodoRepository
    {
        // Fields
        private SqlClientFactory _sqlClientFactory = null;


        // Constructors
        public SqlTodoRepository(SqlClientFactory sqlClientFactory)
        {
            #region Contracts

            if (sqlClientFactory == null) throw new ArgumentException(nameof(sqlClientFactory));

            #endregion

            // Default
            _sqlClientFactory = sqlClientFactory;
        }


        // Methods
        public void Add(Todo todo)
        {
            #region Contracts

            if (todo == null) throw new ArgumentException();

            #endregion

            // Transaction
            using (var transaction = new TransactionScope())
            {
                // Add
                using (var command = _sqlClientFactory.CreateCommand("DefaultConnection"))
                {
                    // CommandParameters
                    command.AddParameter(nameof(todo.TodoId), todo.TodoId, SqlDbType.NVarChar);
                    command.AddParameter(nameof(todo.Name), todo.Name, SqlDbType.NVarChar);
                    command.AddParameter(nameof(todo.IsComplete), todo.IsComplete, SqlDbType.Bit);

                    // CommandText
                    command.CommandText = @"INSERT INTO [dbo].[Todos_Todo]
                                               ([TodoId]
                                               ,[Name]
                                               ,[IsComplete])
                                        VALUES (@TodoId
                                               ,@Name
                                               ,@IsComplete)
                                       ";

                    // Execute
                    command.ExecuteNonQuery();
                }

                // Complete
                transaction.Complete();
            }
        }

        public void Update(Todo todo)
        {
            throw new NotImplementedException();
        }

        public void Remove(string todoId)
        {
            throw new NotImplementedException();
        }

        public Todo FindById(string todoId)
        {
            throw new NotImplementedException();
        }

        public List<Todo> FindAll()
        {
            // FindAll
            using (var command = _sqlClientFactory.CreateCommand("DefaultConnection"))
            {
                // CommandParameters

                // CommandText
                command.CommandText = @"SELECT  [TodoId]
                                               ,[Name]
                                               ,[IsComplete]
                                          FROM  [dbo].[Todos_Todo]";

                // Execute
                return command.ExecuteParseAll<Todo>();
            }
        }

        public TodoCounts CountAll()
        {
            throw new NotImplementedException();
        }
    }
}
