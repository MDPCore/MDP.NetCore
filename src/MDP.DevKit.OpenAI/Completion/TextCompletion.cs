using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class TextCompletion
    {
        // Properties
        public string Id { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public List<TextMessageChoice> Choices { get; set; } = null;

        public CompletionUsage Usage { get; set; } = null;
    }
}
