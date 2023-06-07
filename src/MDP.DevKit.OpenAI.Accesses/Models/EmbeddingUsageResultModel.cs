using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    internal class EmbeddingUsageResultModel
    {
        // Properties
        public int prompt_tokens { get; set; } = 0;

        public int total_tokens { get; set; } = 0;


        // Methods
        public EmbeddingUsage ToEmbeddingUsage()
        {
            // Create
            var embeddingUsage = new EmbeddingUsage()
            {
                PromptTokens = this.prompt_tokens,
                TotalTokens = this.total_tokens,
            };

            // Return
            return embeddingUsage;
        }
    }
}
