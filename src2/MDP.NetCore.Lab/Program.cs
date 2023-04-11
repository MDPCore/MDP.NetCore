using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MDP.Tracing;
using MyLab.Modules;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(WorkService workService, ILogger<Program> logger, ITracer<Program> tracer)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException($"{nameof(workService)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");
            if (tracer == null) throw new ArgumentException($"{nameof(tracer)}=null");

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
