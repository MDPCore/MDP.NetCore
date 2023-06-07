using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class CompletionUsage
    {
        // Properties
        public int PromptTokens { get; set; } = 0;

        public int CompletionTokens { get; set; } = 0;

        public int TotalTokens { get; set; } = 0;        
    }
}
