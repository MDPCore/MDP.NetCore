using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Todos
{
    public interface ITodoRepository
    {
        // Methods
        public void Add(Todo todo);

        public void Update(Todo todo);

        public void Remove(string todoId);

        public Todo FindByTodoId(string todoId);

        public List<Todo> FindAll();
    }
}
