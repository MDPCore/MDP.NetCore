using Autofac;
using SleepZone.Todos.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using SleepZone.Todos.Accesses;
using Quartz;

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

            // TodoContextJob
            container.ScheduleJob<TodoContextJob>((trigger) =>
            {
                // Trigger
                trigger.WithCronSchedule("0/1 * * * * ?");
            });

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

            // SnapshotRepository
            {
                // SnapshotRepository
                container.RegisterNamed<SnapshotRepository>(componentContext =>
                {
                    return "Mock";
                });

                // SqlSnapshotRepository
                container.RegisterType<SqlSnapshotRepository>().Named<SnapshotRepository>("Sql");

                // MockSnapshotRepository
                container.RegisterType<MockSnapshotRepository>().Named<SnapshotRepository>("Mock");
            }
        }
    }
}
