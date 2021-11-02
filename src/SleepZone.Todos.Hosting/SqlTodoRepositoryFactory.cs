using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Accesses;

namespace SleepZone.Todos.Hosting
{
    public class SqlTodoRepositoryFactory : Factory<TodoRepository, SqlTodoRepository>
    {
        // Methods
        protected override SqlTodoRepository CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Create
            return new SqlTodoRepository
            (
                componentContext.Resolve<SqlClientFactory>()
            );
        }
    }
}
