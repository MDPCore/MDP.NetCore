using Autofac;
using CLK.Autofac;
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
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public TodoContextModule(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Default
            _configuration = configuration;
        }


        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // TodoContext
            container.RegisterServiceType<TodoContext, TodoContext, TodoContextFactory, TodoContextOptions>(_configuration, (builder) => builder.SingleInstance());
            
            // Accesses
            container.RegisterServiceType<TodoRepository, SqlTodoRepository, SqlTodoRepositoryFactory>();
            container.RegisterServiceType<SnapshotRepository, SqlSnapshotRepository, SqlSnapshotRepositoryFactory>();

            // Mocks
            container.RegisterServiceType<TodoRepository, MockTodoRepository, MockTodoRepositoryFactory, MockTodoRepositoryOptions>(_configuration);
            container.RegisterServiceType<SnapshotRepository, MockSnapshotRepository, MockSnapshotRepositoryFactory, MockSnapshotRepositoryOptions>(_configuration);
        }
    }
}
