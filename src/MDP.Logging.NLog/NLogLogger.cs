using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLogLib = NLog;

namespace MDP.Logging.NLog
{
    public class NLogLogger<TCategory> : Logger<TCategory>
    {
        // Fields
        private readonly NLogLib.Logger _logger = null;


        // Constructors
        internal NLogLogger()
        {
            // Logger
            _logger = NLogLib.LogManager.GetLogger(typeof(TCategory).FullName);
            if (_logger == null) throw new InvalidOperationException("logger=null");
        }


        // Methods
        public void Debug(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Log
            _logger.Debug(exception, message);
        }

        public void Info(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Log
            _logger.Info(exception, message);
        }

        public void Warn(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Log
            _logger.Warn(exception, message);
        }

        public void Error(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Log
            _logger.Error(exception, message);
        }

        public void Fatal(string message, Exception exception = null, [CallerMemberName]string methodName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(message) == true) throw new ArgumentException();

            #endregion

            // Log
            _logger.Fatal(exception, message);
        }
    }
}
