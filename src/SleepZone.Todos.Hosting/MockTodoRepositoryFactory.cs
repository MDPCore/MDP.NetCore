using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Mocks;

namespace SleepZone.Todos.Hosting
{
    public class MockTodoRepositoryFactory : Factory<TodoRepository, MockTodoRepository>
    {
        // Properties
        public List<Todo> TodoList { get; set; } = new List<Todo>();


        // Methods
        protected override MockTodoRepository CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
          
            #endregion

            // Repository
            var repository = new MockTodoRepository();

            // TodoList
            foreach (var todo in this.TodoList)
            {
                // Add
                repository.Add(new Todo()
                {
                    TodoId = todo.TodoId,
                    Name = todo.Name,
                    IsComplete = todo.IsComplete
                });
            }

            // Return
            return repository;
        }
    }
}
