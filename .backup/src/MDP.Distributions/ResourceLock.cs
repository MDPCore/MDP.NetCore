using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions
{
    public class ResourceLock
    {
        // Properties
        public string ResourceId { get; set; } = string.Empty;

        public string LockerId { get; set; } = string.Empty;

        public DateTime ExpiredTime { get; set; } = DateTime.Now;

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
