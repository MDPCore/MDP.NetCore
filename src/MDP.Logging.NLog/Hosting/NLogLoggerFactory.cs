using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.NLog
{
    public class NLogLoggerFactory : ServiceFactory<IServiceCollection, NLogLoggerSetting>
    {
        // Constructors
        public NLogLoggerFactory() : base("Logging", "NLogLogger") { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, NLogLoggerSetting setting)
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
