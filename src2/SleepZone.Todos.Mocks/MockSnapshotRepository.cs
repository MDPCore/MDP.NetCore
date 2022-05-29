using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLK.Mocks;

namespace SleepZone.Todos.Mocks
{
    public class MockSnapshotRepository : MockRepository<Snapshot, string>, SnapshotRepository
    {
        // Constructors
        public MockSnapshotRepository() : base(snapshot => Tuple.Create(snapshot.SnapshotId))
        {
            // Default

        }


        // Methods
        public Snapshot FindLast()
        {
            // FindLast
            return this.EntityList.OrderByDescending(x => x.CreateTime).FirstOrDefault();
        }
    }
}
