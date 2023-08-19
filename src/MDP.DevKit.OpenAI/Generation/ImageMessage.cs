using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class ImageMessage : IDisposable
    {
        // Constructors
        public void Dispose()
        {
            // Dispose
            this.Image?.Dispose();
        }


        // Properties
        public MemoryStream Image { get; set; } = null;
    }
}
