using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Data.MSSql
{
    public class SqlClientEndpoint
    {
        // Properties
        public string Name { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public List<string> Handlers { get; set; } = new List<string>();
    }
}
