using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Hosting
{
    public class TodoContextOptions
    {
        // Properties
        public string TodoRepository { get; set; }

        public string SnapshotRepository { get; set; }
    }
}
