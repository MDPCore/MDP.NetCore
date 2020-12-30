using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace MDP.Logging.NLog
{
    public  class FileTargetFactory : NLogTargetFactory
    {
        // Fields
        private readonly string _fileName = null;


        // Constructors
        public FileTargetFactory(string fileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentNullException();

            #endregion

            // Arguments
            _fileName = fileName;
        }


        // Methods
        public void Process(Target target)
        {
            #region Contracts

            if (target == null) throw new ArgumentNullException();

            #endregion

            // Require
            if (target is AsyncTargetWrapper) target = (target as AsyncTargetWrapper).WrappedTarget;

            // FileTarget
            var fileTarget = target as FileTarget;
            if (fileTarget == null) return;

            // ConnectionString
            fileTarget.FileName = _fileName;
        }
    }
}
