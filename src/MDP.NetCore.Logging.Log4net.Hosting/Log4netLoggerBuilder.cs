using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.Log4net.Hosting
{
    public class Log4netLoggerBuilder : MDP.NetCore.ServiceBuilder<Log4netLoggerSetting>
    {
        // Constructors
        public Log4netLoggerBuilder()
        {
            // Default
            this.ServiceNamespace = "Logging";
            this.ServiceName = "Log4netLogger";
        }


        // Methods
        protected override void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection, Log4netLoggerSetting loggerSetting)
        {
            #region Contracts

            if (hostBuilderContext == null) throw new ArgumentException($"{nameof(hostBuilderContext)}=null");
            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (loggerSetting == null) throw new ArgumentException($"{nameof(loggerSetting)}=null");

            #endregion

            // Log4net
            serviceCollection.AddLog4netLogger(loggerSetting);
        }
    }
}
