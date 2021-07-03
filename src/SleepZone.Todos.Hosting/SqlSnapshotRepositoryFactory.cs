using CLK.Data.SqlClient;
using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Accesses;

namespace SleepZone.Todos.Hosting
{
    public class SqlSnapshotRepositoryFactory : Factory<SqlSnapshotRepository>
    {
        // Constructors
        public SqlSnapshotRepositoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override SqlSnapshotRepository Create(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Return
            return componentContext.Resolve<SqlSnapshotRepository>();
        }
    }
}
