using MDP.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace MDP.Logging.Log4net
{
    public class Log4netLoggerFactory : ServiceFactory<IServiceCollection, Log4netLoggerSetting>
    {
        // Constructors
        public Log4netLoggerFactory() : base("Logging", "Log4netLogger") { }


        // Methods
        public override void ConfigureService(IServiceCollection serviceCollection, Log4netLoggerSetting setting)
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
