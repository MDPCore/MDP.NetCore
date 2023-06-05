using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public interface ModelRepository
    {
        // Methods
        List<Model> FindAll();

        Model? FindByModelId(string modelId);
    }
}
