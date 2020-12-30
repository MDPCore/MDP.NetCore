using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Settings.Accesses
{
    public class AppSettingRepository : SettingRepository
    {
        // Fields
        private readonly object _syncRoot = new object();

        private readonly Dictionary<string, Setting> _settingDictionary = new Dictionary<string, Setting>();


        // Constructors
        public AppSettingRepository()
        {
            // AppSettings
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                // Require
                if (string.IsNullOrEmpty(key) == true) throw new InvalidOperationException("key=null");

                // AppSetting
                var appSettingKey = key.Trim();
                var appSettingValue = ConfigurationManager.AppSettings[key];
                var appSetting = new Setting(appSettingKey, appSettingValue);

                // Add
                _settingDictionary.Add(appSetting.Key, appSetting);
            }
        }


        // Methods
        public void Add(Setting setting)
        {
            #region Contracts

            if (setting == null) throw new ArgumentException();

            #endregion

            // Sync
            lock(_syncRoot)
            {
                // Nothing

            }
        }

        public void Remove(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Nothing

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
            // Sync
            lock (_syncRoot)
            {
                // Return
                return _settingDictionary.Values.ToList();
            }            
        }
    }
}
