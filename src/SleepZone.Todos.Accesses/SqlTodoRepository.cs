using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Accesses
{
    public class SqlTodoRepository : TodoRepository
    {
        // Constructors
        public SqlTodoRepository()
        {

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
