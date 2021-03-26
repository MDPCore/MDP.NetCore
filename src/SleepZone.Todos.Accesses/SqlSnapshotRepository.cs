using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Accesses
{
    public class SqlSnapshotRepository : SnapshotRepository
    {
        // Constructors
        public SqlSnapshotRepository() 
        {
           
        }


        // Methods
        public void Add(Snapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public Snapshot FindLast()
        {
            throw new NotImplementedException();
        }

        public List<Snapshot> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
