using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.Serilog
{
    public class SerilogLoggerSetting
    {
        // Properties
        public string ConfigFileName { get; set; } = "serilog.json";

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}