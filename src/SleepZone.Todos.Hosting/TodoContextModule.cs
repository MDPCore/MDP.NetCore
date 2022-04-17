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
            container.RegisterFactory<TodoContext, TodoContextFactory>(this.Configuration).SingleInstance();

            // TodoRepository
            container.RegisterFactory<TodoRepository, MockTodoRepositoryFactory>(this.Configuration);
            container.RegisterFactory<TodoRepository, SqlTodoRepositoryFactory>(this.Configuration);

            // SnapshotRepository
            container.RegisterFactory<SnapshotRepository, MockSnapshotRepositoryFactory>(this.Configuration);
            container.RegisterFactory<SnapshotRepository, SqlSnapshotRepositoryFactory>(this.Configuration);
        }
    }
}
