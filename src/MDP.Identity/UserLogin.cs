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
        public string UserId { get; set; } = null;

        public string LoginType { get; set; } = null;

        public string LoginValue { get; set; } = null;

        public DateTime ExpiredAt { get; set; } = DateTime.MaxValue;
    }
}
