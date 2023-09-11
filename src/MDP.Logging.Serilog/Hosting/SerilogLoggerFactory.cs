using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.Serilog
{
    [MDP.Registration.Factory<IServiceCollection, SerilogLoggerSetting>("Logging", "SerilogLogger")]
    public class SerilogLoggerFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, SerilogLoggerSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Serilog
            serviceCollection.AddSerilogLogger(setting);
        }
    }
}
