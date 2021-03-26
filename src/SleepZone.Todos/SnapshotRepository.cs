using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public interface SnapshotRepository
    {
        // Methods
        void Add(Snapshot snapshot);

        Snapshot FindLast();

        List<Snapshot> FindAll();
    }
}
