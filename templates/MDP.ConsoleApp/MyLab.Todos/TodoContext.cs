using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyLab.Todos
{
    [Service<TodoContext>(singleton: true)]
    public class TodoContext
    {
        // Constructors
        public TodoContext(ITodoRepository todoRepository)
        {
            #region Contracts

            if (todoRepository == null) throw new ArgumentException($"{nameof(todoRepository)}=null");

            #endregion

            // Default
            this.TodoRepository = todoRepository;
        }


        // Properties
        public ITodoRepository TodoRepository { get; set; }
    }
}
