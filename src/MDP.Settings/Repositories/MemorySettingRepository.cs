using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDP.Settings
{
    public class MemorySettingRepository : SettingRepository
    {
        // Fields
        private readonly object _syncRoot = new object();

        private readonly bool _isReadonly = true;

        private readonly Dictionary<string, Setting> _settingDictionary = new Dictionary<string, Setting>();


        // Constructors
        public MemorySettingRepository(List<Setting> settingList, bool isReadonly = true)
        {
            #region Contracts

            if (settingList == null) throw new ArgumentException();

            #endregion

            // Default
            _isReadonly = isReadonly;

            // SettingDictionary
            foreach (var setting in settingList)
            {
                // Add
                _settingDictionary.Add(setting.Key, setting);
            }
        }


        // Methods
        public void Add(Setting setting)
        {
            #region Contracts

            if (setting == null) throw new ArgumentException();

            #endregion

            // Require
            if (_isReadonly == true) return;

            // Sync
            lock (_syncRoot)
            {
                // Remove
                this.Remove(setting.Key);

                // Add
                _settingDictionary.Add(setting.Key, setting);
            }
        }

        public void Remove(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // Require
            if (_isReadonly == true) return;

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_settingDictionary.ContainsKey(key) == false) return;

                // Remove
                _settingDictionary.Remove(key);
            }
        }

        public Setting FindByKey(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_settingDictionary.ContainsKey(key) == false) return null;

                // Return
                return _settingDictionary[key];
            }
        }


        public List<Setting> FindAll()
        {
            // Return
            return _settingDictionary.Values.ToList();
        }
    }
}
