using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface ChatCompletionService
    {
        // Methods
        Task<ChatCompletion> CreateAsync(List<ChatMessage> messages, string model = "gpt-3.5-turbo", float temperature = 1, int maxTokens = 16);
    }
}
