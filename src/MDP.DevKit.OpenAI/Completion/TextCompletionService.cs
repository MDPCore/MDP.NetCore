using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface TextCompletionService
    {
        // Methods
        Task<TextCompletion> CreateAsync(string prompt, string model = "text-davinci-003", float temperature = 1, int maxTokens = 16);
    }
}
