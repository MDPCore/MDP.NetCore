using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.Serilog
{
    public class SerilogLoggerFactory : Factory<IServiceCollection, SerilogLoggerSetting>
    {
        // Constructors
        public SerilogLoggerFactory() : base("Logging", "SerilogLogger") { }


        // Methods
        public override List<ServiceRegistration> ConfigureService(IServiceCollection serviceCollection, SerilogLoggerSetting setting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Serilog
            serviceCollection.AddSerilogLogger(setting);

            // Return
            return null;
        }
    }
}
