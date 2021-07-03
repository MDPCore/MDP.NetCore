using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Accesses
{
    public class SqlSnapshotRepository : SnapshotRepository
    {
        // Fields
        private SqlClientFactory _sqlClientFactory = null;


        // Constructors
        public SqlSnapshotRepository(SqlClientFactory sqlClientFactory)
        {
            #region Contracts

            if (sqlClientFactory == null) throw new ArgumentException(nameof(sqlClientFactory));

            #endregion

            // Default
            _sqlClientFactory = sqlClientFactory;
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
