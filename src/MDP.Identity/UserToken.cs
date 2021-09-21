using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class UserToken
    {
        // Properties
        public string UserId { get; set; }

        public string TokenType { get; set; }

        public string TokenValue { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
