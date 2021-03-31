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
using Microsoft.Extensions.Configuration;

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
            container.RegisterInterface<TodoRepository>();
            {
                // MockTodoRepository
                container.RegisterNamed<MockTodoRepository, TodoRepository>();

                // SqlTodoRepository
                container.RegisterNamed<SqlTodoRepository, TodoRepository>();
            }

            // SnapshotRepository
            container.RegisterInterface<SnapshotRepository>();
            {
                // MockSnapshotRepository
                container.RegisterNamed<MockSnapshotRepository, SnapshotRepository>();

                // SqlSnapshotRepository
                container.RegisterNamed<SqlSnapshotRepository, SnapshotRepository>();
            }
        }
    }
}
