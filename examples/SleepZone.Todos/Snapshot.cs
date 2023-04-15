using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public class Snapshot
    {
        // Properties
        public string SnapshotId { get; set; }

        public int TotalCount { get; set; }

        public int CompleteCount { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
