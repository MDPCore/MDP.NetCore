using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Diagnostics
{
    public class TracerActivity : IDisposable
    {
        // Fields
        private readonly Activity? _activity = null;


        // Constructors
        internal TracerActivity(Activity? activity = null)
        {
            // Default
            _activity = activity;
        }

        public void Dispose()
        {
            // Dispose
            _activity?.Dispose();
        }


        // Methods
        public void AddTag(string key, string? value)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentNullException($"{nameof(key)}=null");

            #endregion

            // AddTag
            _activity?.AddTag(key, value);
        }

        public void AddTag(string key, object? value)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentNullException($"{nameof(key)}=null");

            #endregion

            // AddTag
            _activity?.AddTag(key, value);
        }
    }
}
