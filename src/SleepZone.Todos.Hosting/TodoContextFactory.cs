using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextFactory : Factory<TodoContext, TodoContextOptions>
    {
        // Constructors
        public TodoContextFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override TodoContext Create(IComponentContext componentContext, TodoContextOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Create
            var context = new TodoContext
            (
                componentContext.ResolveNamed<TodoRepository>(options.TodoRepository),
                componentContext.ResolveNamed<SnapshotRepository>(options.SnapshotRepository)
            );

            // Return
            return context;
        }
    }
}
