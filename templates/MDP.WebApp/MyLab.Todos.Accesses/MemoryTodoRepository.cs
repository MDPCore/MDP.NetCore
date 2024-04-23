using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Todos.Accesses
{
    [Service<ITodoRepository>()]
    public class MemoryTodoRepository : ITodoRepository
    {
        // Fields
        private readonly List<Todo> _todoList = new List<Todo>();


        // Methods
        public void Add(Todo todo)
        {
            #region Contracts

            if (todo == null) throw new ArgumentException($"{nameof(todo)}=null");

            #endregion

            // Add
            _todoList.RemoveAll(o => o.TodoId == todo.TodoId);
            _todoList.Add(todo);
        }

        public void Update(Todo todo)
        {
            #region Contracts

            if (todo == null) throw new ArgumentException($"{nameof(todo)}=null");

            #endregion

            // Update
            _todoList.RemoveAll(o=>o.TodoId == todo.TodoId);
            _todoList.Add(todo);
        }

        public void Remove(string todoId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(todoId) == true) throw new ArgumentException($"{nameof(todoId)}=null");

            #endregion

            // Remove
            _todoList.RemoveAll(o => o.TodoId == todoId);
        }

        public Todo FindByTodoId(string todoId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(todoId) == true) throw new ArgumentException($"{nameof(todoId)}=null");

            #endregion

            // FindByTodoId
            return _todoList.FirstOrDefault(o => o.TodoId == todoId);
        }

        public List<Todo> FindAll()
        {
            // FindAll
            return _todoList.ToList();
        }
    }
}