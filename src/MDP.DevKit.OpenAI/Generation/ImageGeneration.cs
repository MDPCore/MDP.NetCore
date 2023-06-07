using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class ImageGeneration : IDisposable
    {
        // Constructors
        public void Dispose()
        {
            // Dispose
            this.Data.ForEach(o => o?.Dispose());
        }


        // Properties
        public List<ImageMessage> Data { get; set; } = new List<ImageMessage>();
    }
}
