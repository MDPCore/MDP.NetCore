using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging
{
    public interface Logger
    {
        // Methods
        void Debug(string message, Exception exception = null, [CallerMemberName]string methodName = "");

        void Info(string message, Exception exception = null, [CallerMemberName]string methodName = "");

        void Warn(string message, Exception exception = null, [CallerMemberName]string methodName = "");

        void Error(string message, Exception exception = null, [CallerMemberName]string methodName = "");

        void Fatal(string message, Exception exception = null, [CallerMemberName]string methodName = "");
    }

    public interface Logger<TCategory> : Logger
    {

    }
}
