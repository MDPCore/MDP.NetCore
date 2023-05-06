using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace WebApplication1
{
    [MDP.Registration.Factory<WebApplicationBuilder, Setting>("Logging", "Serilog")]
    public class SerilogFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, Setting setting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // UseSerilog
            webApplicationBuilder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
            {
                // MinimumLevel
                loggerConfiguration.MinimumLevel.Error();

                // WriteToFile
                var entryDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
                var LogFilePah = Path.Combine(entryDirectoryPath, setting.LogFile);
                loggerConfiguration.WriteTo.File(LogFilePah);
            });
        }


        // Class
        public class Setting
        {
            // Properties
            public string LogFile { get; set; } = string.Empty;
        }
    }
}
