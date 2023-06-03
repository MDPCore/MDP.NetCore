using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.NLog
{
    [MDP.Registration.Factory<IServiceCollection, NLogLoggerSetting>("Logging", "NLogLogger")]
    public class NLogLoggerFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, NLogLoggerSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // NLog
            serviceCollection.AddNLogLogger(setting);
        }
    }
}
