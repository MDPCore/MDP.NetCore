using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging
{
    public interface ILogger
    {
        // Debug
        void LogDebug(string message, params object[] args);

        void LogDebug(Exception exception, string message, params object[] args);

        // Trace
        void LogTrace(string message, params object[] args);

        void LogTrace(Exception exception, string message, params object[] args);

        // Information
        void LogInformation(string message, params object[] args);

        void LogInformation(Exception exception, string message, params object[] args);

        // Warning
        void LogWarning(string message, params object[] args);

        void LogWarning(Exception exception, string message, params object[] args);

        // Error
        void LogError(string message, params object[] args);

        void LogError(Exception exception, string message, params object[] args);

        // Critical
        void LogCritical(string message, params object[] args);

        void LogCritical(Exception exception, string message, params object[] args);
    }

    public interface ILogger<TCategory> : ILogger
    {
        
    }
}
