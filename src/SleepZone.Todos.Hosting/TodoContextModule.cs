using Autofac;
using SleepZone.Todos.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using SleepZone.Todos.Accesses;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextModule : MDP.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // TodoContext
            {
                // Register
                container.RegisterType<TodoContext>().As<TodoContext>()

                // Start
                .OnActivated((handler) =>
                {

                })

                // Lifetime
                .AutoActivate().SingleInstance();
            }

            // TodoRepository
            {
                // TodoRepository
                container.RegisterNamed<TodoRepository>(componentContext =>
                {
                    return "Mock";
                });

                // SqlTodoRepository
                container.RegisterType<SqlTodoRepository>().Named<TodoRepository>("Sql");

                // MockTodoRepository
                container.RegisterType<MockTodoRepository>().Named<TodoRepository>("Mock");
            }

            // TodoSnapshotRepository
            {
                // TodoSnapshotRepository
                container.RegisterNamed<TodoSnapshotRepository>(componentContext =>
                {
                    return "Mock";
                });

                // SqlTodoSnapshotRepository
                container.RegisterType<SqlTodoSnapshotRepository>().Named<TodoSnapshotRepository>("Sql");

                // MockTodoSnapshotRepository
                container.RegisterType<MockTodoSnapshotRepository>().Named<TodoSnapshotRepository>("Mock");
            }
        }
    }
}
