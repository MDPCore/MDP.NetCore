using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Mocks;

namespace SleepZone.Todos.Hosting
{
    public class MockSnapshotRepositoryFactory : Factory<SnapshotRepository, MockSnapshotRepository, MockSnapshotRepositoryFactory.Setting>
    {
        // Methods
        protected override MockSnapshotRepository CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new MockSnapshotRepository();
        }


        // Class
        public class Setting
        {
           
        }
    }
}
