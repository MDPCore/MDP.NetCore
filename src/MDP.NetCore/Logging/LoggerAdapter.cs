using Microsoft.Extensions.Logging;
using System;

namespace MDP.NetCore
{
    internal class LoggerAdapter<TCategory> : MDP.Logging.ILogger<TCategory>
    {
        // Fields
        private readonly Microsoft.Extensions.Logging.ILogger<TCategory> _logger;


        // Constructors
        public LoggerAdapter(Microsoft.Extensions.Logging.ILogger<TCategory> logger)
        {
            #region Contracts

            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");

            #endregion

            // Default
            _logger = logger;
        }


        // Debug
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogDebug(Exception exception, string message, params object[] args)
        {
            _logger.LogDebug(exception, message, args);
        }

        // Trace
        public void LogTrace(string message, params object[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogTrace(Exception exception, string message, params object[] args)
        {
            _logger.LogTrace(exception, message, args);
        }

        // Information
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        // Warning
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        // Error
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        // Critical
        public void LogCritical(string message, params object[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }
}
