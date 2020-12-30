using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace MDP.Logging
{
    internal class LoggerCollection<TCategory> : Logger<TCategory>
    {
        // Fields
        private readonly IEnumerable<Logger<TCategory>> _loggerList = null;


        // Constructors
        public LoggerCollection(IEnumerable<Logger<TCategory>> loggerList)
        {
            #region Contracts

            if (loggerList == null) throw new ArgumentException();

            #endregion

            // Default
            _loggerList = loggerList;
        }


        // Methods
        public void Debug(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Exception
            while (true)
            {
                if (exception == null) break;
                if (exception.InnerException == null) break;
                exception = exception.InnerException;
            }

            // Log
            foreach (var logger in _loggerList)
            {
                logger.Debug(message, exception, methodName);
            }
        }

        public void Info(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Exception
            while (true)
            {
                if (exception == null) break;
                if (exception.InnerException == null) break;
                exception = exception.InnerException;
            }

            // Log
            foreach (var logger in _loggerList)
            {
                logger.Info(message, exception, methodName);
            }
        }

        public void Warn(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Exception
            while (true)
            {
                if (exception == null) break;
                if (exception.InnerException == null) break;
                exception = exception.InnerException;
            }

            // Log
            foreach (var logger in _loggerList)
            {
                logger.Warn(message, exception, methodName);
            }
        }

        public void Error(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Exception
            while (true)
            {
                if (exception == null) break;
                if (exception.InnerException == null) break;
                exception = exception.InnerException;
            }

            // Log
            foreach (var logger in _loggerList)
            {
                logger.Error(message, exception, methodName);
            }
        }

        public void Fatal(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Exception
            while (true)
            {
                if (exception == null) break;
                if (exception.InnerException == null) break;
                exception = exception.InnerException;
            }

            // Log
            foreach (var logger in _loggerList)
            {
                logger.Fatal(message, exception, methodName);
            }
        }
    }
}
