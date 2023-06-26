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


        // Methods
        public HttpClientEndpoint Clone()
        {
            // Create
            var endpoint = new HttpClientEndpoint();
            endpoint.Name = this.Name;
            endpoint.BaseAddress = this.BaseAddress;
            endpoint.Headers = new Dictionary<string, string>(this.Headers);

            // Return
            return endpoint;
        }
    }
}
