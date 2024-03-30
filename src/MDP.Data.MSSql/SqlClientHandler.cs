using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.MSSql
{
    public interface SqlClientHandler
    {
        // Methods
        void Handle(SqlClient sqlClient);
    }
}
