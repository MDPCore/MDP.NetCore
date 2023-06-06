using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface CompletionService
    {
        // Methods
        Task<Completion> CreateAsync(string model, string prompt, float temperature = 1, int maxTokens = 16);
    }

    public class Completion
    {
        // Properties
        public string CompletionId { get; set; } = string.Empty;
    }
}
