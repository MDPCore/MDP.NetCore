using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface ModelService
    {
        // Methods
        Task<List<Model>> FindAllAsync();

        Task<Model> FindByIdAsync(string model);
    }
}
