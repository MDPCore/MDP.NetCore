using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextFactory : Factory<TodoContext, TodoContext>
    {
        // Methods
        protected override TodoContext CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Create
            return new TodoContext
            (
                componentContext.Resolve<TodoRepository>(),
                componentContext.Resolve<SnapshotRepository>()
            );
        }
    }
}
