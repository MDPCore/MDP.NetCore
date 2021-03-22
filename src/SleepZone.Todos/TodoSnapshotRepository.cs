using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos
{
    public interface TodoSnapshotRepository
    {
        // Methods
        void Add(TodoSnapshot todoSnapshot);

        List<TodoSnapshot> FindAll();
    }
}
