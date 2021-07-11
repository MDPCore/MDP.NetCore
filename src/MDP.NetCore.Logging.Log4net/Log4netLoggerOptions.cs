using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.Log4net
{
    public class Log4netLoggerOptions
    {
        // Properties
        public string ConfigFileName { get; set; } = "log4net.config";

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
