using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public interface TodoRepository
    {
        // Methods
        void Add(Todo todo);

        void Update(Todo todo);

        void Remove(string todoId);

        Todo FindById(string todoId);

        List<Todo> FindAll();

        TodoCounts CountAll();
    }
}
