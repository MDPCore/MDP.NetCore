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
    public class SqlTodoRepositoryFactory : Factory<TodoRepository, SqlTodoRepository, SqlTodoRepositoryFactory.Setting>
    {
        // Methods
        protected override SqlTodoRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new SqlTodoRepository
            (
                componentContext.Resolve<SqlClientFactory>()
            );
        }


        // Class
        public class Setting
        {

        }
    }
}
