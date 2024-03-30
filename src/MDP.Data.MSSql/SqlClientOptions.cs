using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.MSSql
{
    public class SqlClientOptions
    {
        // Properties
        public string Name { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public List<SqlClientHandler> Handlers { get; set; } = new List<SqlClientHandler>();
    }
}
