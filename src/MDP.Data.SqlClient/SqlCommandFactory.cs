using CLK.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.SqlClient
{
    public interface SqlCommandFactory
    {
        // Methods
        SqlCommandScope Create();

        SqlCommandScope Create(string name);
    }
}
