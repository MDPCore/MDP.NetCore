using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Settings
{
    public interface SettingRepository
    {
        // Methods
        void Add(Setting setting);

        void Remove(string key);


        Setting FindByKey(string key);

        List<Setting> FindAll();
    }
}
