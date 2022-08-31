using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.NLog.Hosting
{
    public class NLogLoggerBuilder : MDP.NetCore.ServiceBuilder<NLogLoggerSetting>
    {
        // Constructors
        public NLogLoggerBuilder()
        {
            // Default
            this.ServiceNamespace = "Logging";
            this.ServiceName = "NLogLogger";
        }


        // Methods
        protected override void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection, NLogLoggerSetting loggerSetting)
        {
            #region Contracts

            if (hostBuilderContext == null) throw new ArgumentException($"{nameof(hostBuilderContext)}=null");
            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (loggerSetting == null) throw new ArgumentException($"{nameof(loggerSetting)}=null");

            #endregion

            // NLog
            serviceCollection.AddNLogLogger(loggerSetting);
        }
    }
}
