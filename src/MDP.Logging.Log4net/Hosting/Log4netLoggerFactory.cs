using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Registration;

namespace MDP.Logging.Log4net
{
    public class Log4netLoggerFactory : Factory<IServiceCollection, Log4netLoggerSetting>
    {
        // Constructors
        public Log4netLoggerFactory() : base("Logging", "Log4netLogger") { }


        // Methods
        public override List<ServiceRegistration> ConfigureService(IServiceCollection serviceCollection, Log4netLoggerSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Log4net
            serviceCollection.AddLog4netLogger(setting);

            // Return
            return null;
        }
    }
}
