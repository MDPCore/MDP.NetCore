using Autofac;
using MDP;
using MDP.Hosting;
using Microsoft.Extensions.Configuration;
using SleepZone.Todos.Accesses;
using SleepZone.Todos.Mocks;
using System;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // TodoContext
            container.RegisterService<TodoContext>().SingleInstance();
            container.RegisterFactory<TodoContext, TodoContextFactory>();

            // TodoRepository
            container.RegisterService<TodoRepository>();
            container.RegisterFactory<TodoRepository, MockTodoRepositoryFactory>();
            container.RegisterFactory<TodoRepository, SqlTodoRepositoryFactory>();

            // SnapshotRepository
            container.RegisterService<SnapshotRepository>();
            container.RegisterFactory<SnapshotRepository, MockSnapshotRepositoryFactory>();
            container.RegisterFactory<SnapshotRepository, SqlSnapshotRepositoryFactory>();
        }
    }
}
