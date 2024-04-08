using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Configuration
{
    public interface ConfigurationProvider
    {
        // Methods
        IEnumerable<Stream> GetAllJsonStream();
    }
}
