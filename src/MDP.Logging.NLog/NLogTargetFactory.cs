using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDP.Logging.NLog
{
    public interface NLogTargetFactory
    {
        // Methods
        void Process(Target target);
    }
}
