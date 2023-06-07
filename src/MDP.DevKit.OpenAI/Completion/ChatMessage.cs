using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class ChatMessage
    {
        // Properties
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
