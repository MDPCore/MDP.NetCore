using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MDP.Hosting;
using Autofac;
using SleepZone.Todos.Mocks;

namespace SleepZone.Todos.Hosting
{
    public class MockSnapshotRepositoryFactory : Factory<MockSnapshotRepository, MockSnapshotRepositoryOptions>
    {
        // Constructors
        public MockSnapshotRepositoryFactory(IComponentContext componentContext) : base(componentContext)
        {

        }


        // Methods
        protected override MockSnapshotRepository Create(IComponentContext componentContext, MockSnapshotRepositoryOptions options)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Repository
            var repository = new MockSnapshotRepository();
                       
            // Return
            return repository;
        }
    }
}
