using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    internal class CompletionUsageResultModel
    {
        // Properties
        public int prompt_tokens { get; set; } = 0;

        public int completion_tokens { get; set; } = 0;

        public int total_tokens { get; set; } = 0;


        // Methods
        public CompletionUsage ToCompletionUsage()
        {
            // Create
            var completionUsage = new CompletionUsage()
            {
                PromptTokens = this.prompt_tokens,
                CompletionTokens = this.completion_tokens,
                TotalTokens = this.total_tokens,
            };

            // Return
            return completionUsage;
        }
    }
}
