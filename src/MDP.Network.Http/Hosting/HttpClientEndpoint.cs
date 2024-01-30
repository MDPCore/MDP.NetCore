using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Network.Http
{
    public class HttpClientEndpoint
    {
        // Properties
        public string Name { get; set; } = string.Empty;

        public string BaseAddress { get; set; } = string.Empty;

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public List<string> Handlers { get; set; } = new List<string>();
    }
}
