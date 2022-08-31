using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextFactory : ServiceFactory<TodoContext, TodoContext, TodoContextFactory.Setting>
    {
        // Constructors
        public TodoContextFactory()
        {
            // Default
            this.ServiceSingleton = true;
        }


        // Methods
        protected override TodoContext CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new TodoContext
            (
                componentContext.Resolve<TodoRepository>(setting.TodoRepository),
                componentContext.Resolve<SnapshotRepository>(setting.SnapshotRepository)
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string TodoRepository { get; set; } = String.Empty;

            public string SnapshotRepository { get; set; } = String.Empty;
        }
    }
}
