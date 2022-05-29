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

        private readonly SnapshotRepository _snapshotRepository = null;

        private readonly SnapshotService _snapshotService = null;


        // Constructors
        public TodoContext(TodoRepository todoRepository, SnapshotRepository snapshotRepository)
        {
            #region Contracts

            if (todoRepository == null) throw new ArgumentException(nameof(todoRepository));
            if (snapshotRepository == null) throw new ArgumentException(nameof(snapshotRepository));

            #endregion

            // Default
            _todoRepository = todoRepository;
            _snapshotRepository = snapshotRepository;
            _snapshotService = new SnapshotService(todoRepository, snapshotRepository);
        }


        // Properties
        public TodoRepository TodoRepository { get { return _todoRepository; } }

        public SnapshotRepository SnapshotRepository { get { return _snapshotRepository; } }

        public SnapshotService SnapshotService { get { return _snapshotService; } }
    }
}
