using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.NLog
{
    public class NLogLoggerSetting
    {
        // Properties
        public string ConfigFileName { get; set; } = "nlog.config";

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}