using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLK.Mocks;

namespace SleepZone.Todos.Mocks
{
    public class MockTodoSnapshotRepository : MockRepository<TodoSnapshot, string>, TodoSnapshotRepository
    {
        // Constructors
        public MockTodoSnapshotRepository() : base(todoSnapshot => todoSnapshot.TodoSnapshotId)
        {
            // Default

        }
    }
}
