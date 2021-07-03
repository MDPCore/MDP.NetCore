using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Mocks;

namespace SleepZone.Todos.Hosting
{
    public class MockTodoRepositoryFactory : Factory<MockTodoRepository, MockTodoRepositoryOptions>
    {
        // Constructors
        public MockTodoRepositoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override MockTodoRepository Create(IComponentContext componentContext, MockTodoRepositoryOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Repository
            var repository = new MockTodoRepository();

            // TodoList
            foreach (var todoOptions in options.TodoList)
            {
                // Add
                repository.Add(new Todo()
                {
                    TodoId = todoOptions.TodoId,
                    Name = todoOptions.Name,
                    IsComplete = todoOptions.IsComplete
                });
            }

            // Return
            return repository;
        }
    }
}
