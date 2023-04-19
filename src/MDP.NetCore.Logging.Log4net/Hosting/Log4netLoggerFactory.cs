using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.Log4net
{
    [MDP.Registration.Factory<IServiceCollection, Log4netLoggerSetting>("Logging", "Log4netLogger")]
    public class Log4netLoggerFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, Log4netLoggerSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Log4net
            serviceCollection.AddLog4netLogger(setting);
        }
    }
}
