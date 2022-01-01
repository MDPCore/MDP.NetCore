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
    public class MockTodoRepositoryFactory : Factory<TodoRepository, MockTodoRepository, MockTodoRepositoryFactory.Setting>
    {
        // Methods
        protected override MockTodoRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Repository
            var repository = new MockTodoRepository();

            // TodoList
            foreach (var todo in setting.TodoList)
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


        // Class
        public class Setting
        {
            // Properties
            public List<Todo> TodoList { get; set; } = new List<Todo>();
        }
    }
}
