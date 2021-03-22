using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public class TodoContext
    {
        // Fields
        private readonly TodoRepository _todoRepository = null;

        private readonly TodoSnapshotRepository _todoSnapshotRepository = null;


        // Constructors
        public TodoContext(TodoRepository todoRepository, TodoSnapshotRepository todoSnapshotRepository)
        {
            #region Contracts

            if (todoRepository == null) throw new ArgumentException(nameof(todoRepository));
            if (todoSnapshotRepository == null) throw new ArgumentException(nameof(todoSnapshotRepository));

            #endregion

            // Default
            _todoRepository = todoRepository;
            _todoSnapshotRepository = todoSnapshotRepository;
        }


        // Properties
        public TodoRepository TodoRepository { get { return _todoRepository; } }

        public TodoSnapshotRepository TodoSnapshotRepository { get { return _todoSnapshotRepository; } }


        // Methods
        public void Snapshot()
        {
            // TodoCounts
            var todoCounts = this.TodoRepository.CountAll();
            if (todoCounts == null) throw new InvalidOperationException(nameof(todoCounts));

            // TodoSnapshot
            var todoSnapshot = new TodoSnapshot()
            {
                TodoSnapshotId = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                TotalCount = todoCounts.TotalCount,
                CompleteCount = todoCounts.CompleteCount
            };

            // Add
            this.TodoSnapshotRepository.Add(todoSnapshot);
        }
    }
}
