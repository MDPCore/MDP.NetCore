using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Accesses
{
    public class SqlTodoSnapshotRepository : TodoSnapshotRepository
    {
        // Constructors
        public SqlTodoSnapshotRepository() 
        {
           
        }


        // Methods
        public void Add(TodoSnapshot todoSnapshot)
        {
            throw new NotImplementedException();
        }

        public List<TodoSnapshot> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
