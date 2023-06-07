using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface TextEmbeddingService
    {
        // Methods
        Task<TextEmbedding> CreateAsync(string prompt, string model = "text-embedding-ada-002");
    }
}

