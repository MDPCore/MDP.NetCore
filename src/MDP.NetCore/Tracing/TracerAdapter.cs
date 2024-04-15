﻿using MDP.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    internal abstract class TracerAdapter : ITracer
    {
        // Fields
        private static object _syncRoot = new object();

        private static ActivitySource _activitySource = null;


        // Constructors
        public TracerAdapter()
        {

        }


        // Properties
        private ActivitySource ActivitySource
        {
            get
            {
                // Sync
                lock (_syncRoot)
                {
                    // Create
                    if (_activitySource == null)
                    {
                        // EntryAssemblyName
                        var entryAssemblyName = Assembly.GetExecutingAssembly()?.GetName().Name;
                        if (string.IsNullOrEmpty(entryAssemblyName) == true) throw new InvalidOperationException($"{nameof(entryAssemblyName)}=null");

                        // ActivitySource
                        _activitySource = new ActivitySource(entryAssemblyName);
                    }

                    // Return
                    return _activitySource;
                }

            }
        }


        // Methods
        public TracerActivity Start([CallerMemberName] string memberName = "")
        {
            #region Contracts

            if (string.IsNullOrEmpty(memberName) == true) throw new ArgumentException($"{nameof(memberName)}=null");

            #endregion

            // ActivityName
            var activityName = this.CreateActivityName(memberName);
            if (string.IsNullOrEmpty(activityName) == true) throw new InvalidOperationException($"{nameof(activityName)}=null");

            // Activity
            var activity = this.ActivitySource.StartActivity(activityName);
          
            // Return
            return new TracerActivity(activity);
        }

        protected abstract string CreateActivityName(string memberName);
    }

    internal class TracerAdapter<TCategoryName> : TracerAdapter, ITracer<TCategoryName>
    {
        // Fields
        private readonly string _categoryName;


        // Constructors
        public TracerAdapter() 
        {
            // CategoryName
            _categoryName = typeof(TCategoryName).FullName!;
            if (string.IsNullOrEmpty(_categoryName) == true) throw new InvalidOperationException($"{nameof(_categoryName)}=null");
        }


        // Methods
        protected override string CreateActivityName(string memberName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(memberName) == true) throw new ArgumentException($"{nameof(memberName)}=null");

            #endregion

            // ActivityName
            var activityName = $"{_categoryName}.{memberName}";
            if (string.IsNullOrEmpty(activityName) == true) throw new InvalidOperationException($"{nameof(activityName)}=null");

            // Return
            return activityName;
        }
    }
}
