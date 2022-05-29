using CLK.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(WorkService workService, ITracer<Program> tracer, ILogger<Program> logger)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException($"{nameof(workService)}=null");
            if (tracer == null) throw new ArgumentException($"{nameof(tracer)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");

            #endregion

            // Execute
            Console.WriteLine(workService.GetValue());
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}
