using MDP.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace MDP.Quartz
{
    public static class MdpBuilderExtensions
    {
        // Methods
        public static MdpBuilder AddQuartz(this MdpBuilder mdpBuilder)
        {
            #region Contracts

            if (mdpBuilder == null) throw new ArgumentException(nameof(mdpBuilder));

            #endregion

            // ConfigureServices
            mdpBuilder.HostBuilder.ConfigureServices((hostContext, services) =>
            {
                // Quartz
                services.AddQuartz(options =>
                {
                    // ScopedJob
                    options.UseMicrosoftDependencyInjectionScopedJobFactory();
                });

                // QuartzHostedService
                services.AddQuartzHostedService(options => 
                {
                    // WaitForJobsToComplete
                    options.WaitForJobsToComplete = true;
                });
            });

            // Return
            return mdpBuilder;
        }
    }
}
