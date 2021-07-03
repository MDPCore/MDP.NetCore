using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Accesses
{
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public TodoCounts CountAll()
        {
            throw new NotImplementedException();
        }
    }
}
