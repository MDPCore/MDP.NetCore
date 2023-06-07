using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface ImageGenerationService
    {
        // Methods
        Task<ImageGeneration> CreateAsync(string prompt, int n = 1, string size = "1024x1024");
    }
}
