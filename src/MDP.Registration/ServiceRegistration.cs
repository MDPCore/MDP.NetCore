using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public class ServiceRegistration
    {
        // Properties
        public Type BuilderType { get; set; } 

        public Type ServiceType { get; set; }

        public Type InstanceType { get; set; }

        public string InstanceName { get; set; } = null;

        public Dictionary<string, object> Parameters { get; set; } = null;

        public bool Singleton { get; set; } = false;
    }
}
