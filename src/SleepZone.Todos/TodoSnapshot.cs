using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public class TodoSnapshot
    {
        // Properties
        public string TodoSnapshotId { get; set; }

        public int TotalCount { get; set; }

        public int CompleteCount { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
