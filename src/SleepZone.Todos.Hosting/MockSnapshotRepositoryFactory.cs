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
    public class MockSnapshotRepositoryFactory : Factory<SnapshotRepository, MockSnapshotRepository>
    {
        // Methods
        protected override MockSnapshotRepository CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
          
            #endregion

            // Create
            return new MockSnapshotRepository();
        }
    }
}
