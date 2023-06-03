using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.NLog
{
    public class NLogLoggerSetting
    {
        // Properties
        public string ConfigFileName { get; set; } = "nlog.config";

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}