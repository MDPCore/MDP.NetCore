using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Settings
{
    public class Setting
    {
        // Constructors
        public Setting(string key, string value = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // Default
            this.Key = key;
            this.Value = value;
        }


        // Properties
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
