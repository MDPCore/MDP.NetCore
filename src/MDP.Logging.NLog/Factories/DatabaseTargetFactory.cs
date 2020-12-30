using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace MDP.Logging.NLog
{
    public  class DatabaseTargetFactory : NLogTargetFactory
    {
        // Fields
        private readonly string _connectionString = null;


        // Constructors
        public DatabaseTargetFactory(string connectionString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(connectionString) == true) throw new ArgumentNullException();

            #endregion

            // Arguments
            _connectionString = connectionString;
        }


        // Methods
        public void Process(Target target)
        {
            #region Contracts

            if (target == null) throw new ArgumentNullException();

            #endregion

            // Require
            if (target is AsyncTargetWrapper) target = (target as AsyncTargetWrapper).WrappedTarget;

            // DatabaseTarget
            var databaseTarget = target as DatabaseTarget;
            if (databaseTarget == null) return;

            // ConnectionString
            databaseTarget.ConnectionString = _connectionString;
        }
    }
}
