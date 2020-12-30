using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Settings
{
    public class SettingContext : IDisposable
    {
        // Fields
        private readonly List<SettingRepository> _settingRepositoryList = null;


        // Constructors
        public SettingContext(List<SettingRepository> settingRepositoryList)
        {
            #region Contracts

            if (settingRepositoryList == null) throw new ArgumentException();

            #endregion

            // Default
            _settingRepositoryList = settingRepositoryList;
        }

        public void Start()
        {
            // Nothing

        }

        public void Dispose()
        {
            // Nothing

        }


        // Methods
        public void SetValue(string key, string value = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // SettingRepositoryList
            foreach (var settingRepository in _settingRepositoryList)
            {
                // Remove
                settingRepository.Remove(key);

                // Add
                settingRepository.Add(new Setting(key, value));
            }
        }

        public string GetValue(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException();

            #endregion

            // SettingRepositoryList
            foreach (var settingRepository in _settingRepositoryList)
            {
                // Setting
                var setting = settingRepository.FindByKey(key);
                if (setting != null) return setting.Value;
            }

            // Return
            return null;
        }
    }
}
