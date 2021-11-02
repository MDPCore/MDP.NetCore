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
        public string UserId { get; set; } = null;

        public string TokenType { get; set; } = null;

        public string TokenValue { get; set; } = null;

        public DateTime ExpiredAt { get; set; } = DateTime.MinValue;
    }
}
