using MDP.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDP.Settings
{
    public class CacheSettingRepository : SettingRepository
    {
        // // Fields
        private readonly object _syncRoot = new object();

        private readonly SettingRepository _settingRepository = null;

        private readonly int _effectiveDuration = 0;

        private DateTime _effectiveTime = DateTime.MinValue;

        private Dictionary<string, Setting> _settingDictionary = null;


        // Constructors
        public CacheSettingRepository(SettingRepository settingRepository, int effectiveDuration = 60)
        {
            #region Contracts

            if (settingRepository == null) throw new ArgumentException();

            #endregion

            // Default
            _settingRepository = settingRepository;
            _effectiveDuration = effectiveDuration;
        }


        // Methods
        public void Add(Setting setting)
        {
            #region Contracts

            if (setting == null) throw new ArgumentException();

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Cache
                _settingDictionary = null;
            }

            // Add
            _settingRepository.Add(setting);
        }

        public void Remove(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key)) throw new ArgumentException();

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Cache
                _settingDictionary = null;
            }

            // Remove
            _settingRepository.Remove(key);
        }

        public Setting FindByKey(string key)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key)) throw new ArgumentException();

            #endregion

            // SettingListDictionary
            var settingDictionary = this.GetSettingDictionary();
            if (settingDictionary == null) throw new InvalidOperationException();

            // FindByKey
            if (settingDictionary.ContainsKey(key) == true)
            {
                return settingDictionary[key];
            }

            // Return
            return null;
        }

        public List<Setting> FindAll()
        {
            // SettingListDictionary
            var settingDictionary = this.GetSettingDictionary();
            if (settingDictionary == null) throw new InvalidOperationException();

            // Return
            return settingDictionary.Values.ToList();
        }


        private Dictionary<string, Setting> GetSettingDictionary()
        {
            // Sync
            lock (_syncRoot)
            {
                // Cache
                if (_effectiveTime >= DateTime.Now)
                {
                    if (_settingDictionary != null)
                    {
                        return _settingDictionary;
                    }
                }

                // FindAll
                var settingList = _settingRepository.FindAll();
                if (settingList == null) throw new InvalidOperationException();

                // Setting
                _effectiveTime = this.CreateEffectiveTime(DateTime.Now);
                _settingDictionary = settingList.ToDictionary(o => o.Key, o => o);

                // Return
                return _settingDictionary;
            }
        }

        private DateTime CreateEffectiveTime(DateTime executeTime)
        {
            // Result
            var effectiveTime = new DateTime(executeTime.Year, executeTime.Month, executeTime.Day, executeTime.Hour, 0, 0);

            // Add
            do
            {
                effectiveTime = effectiveTime.AddMinutes(_effectiveDuration);
            }
            while (effectiveTime < executeTime);

            // Return
            return effectiveTime;
        }
    }
}