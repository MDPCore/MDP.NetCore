using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class TextEmbedding
    {
        // Properties
        public string Model { get; set; } = string.Empty;

        public List<EmbeddingVector> Data { get; set; } = null;

        public EmbeddingUsage Usage { get; set; } = null;
    }
}
