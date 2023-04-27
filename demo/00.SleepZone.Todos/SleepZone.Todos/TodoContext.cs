using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    [MDP.Registration.Service<TodoContext>(singleton: true)]
    public class TodoContext
    {
        // Fields
        private readonly TodoRepository _todoRepository;

        private readonly SnapshotRepository _snapshotRepository;

        private readonly SnapshotService _snapshotService;


        // Constructors
        public TodoContext(TodoRepository todoRepository, SnapshotRepository snapshotRepository)
        {
            #region Contracts

            if (todoRepository == null) throw new ArgumentException($"{nameof(todoRepository)}=null");
            if (snapshotRepository == null) throw new ArgumentException($"{nameof(snapshotRepository)}=null");

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
