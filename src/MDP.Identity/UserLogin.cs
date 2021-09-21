using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class UserLogin
    {
        // Properties
        public string UserId { get; set; }

        public string LoginType { get; set; }

        public string LoginValue { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
