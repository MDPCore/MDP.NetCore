using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class ChatMessageChoice : ChatMessage
    {
        // Properties
        public int Index { get; set; } = 0;

        public string FinishReason { get; set; } = null;
    }
}
