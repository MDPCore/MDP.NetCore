using MDP.Logging;
using MDP.Tracing;
using MyLab.Modules;
using System;
using System.ComponentModel;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(IServiceProvider serviceProvider, WorkService workServiceA, ILogger<Program> logger, ITracer<Program> tracer)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (workServiceA == null) throw new ArgumentException($"{nameof(workServiceA)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");
            if (tracer == null) throw new ArgumentException($"{nameof(tracer)}=null");

            #endregion

            // ResolveB
            var workServiceB = serviceProvider.ResolveTyped<WorkService>();
            if (workServiceB == null) throw new InvalidOperationException($"{nameof(workServiceB)}=null");

            // ResolveC
            var workServiceC = serviceProvider.ResolveNamed<WorkService>("HelloWorkService");
            if (workServiceC == null) throw new InvalidOperationException($"{nameof(workServiceC)}=null");

            // ResolveD
            var workServiceD = serviceProvider.ResolveNamed<WorkService>("HelloWorkService[456]");
            if (workServiceD == null) throw new InvalidOperationException($"{nameof(workServiceD)}=null");

            // ResolveE
            var workServiceE = serviceProvider.ResolveNamed<WorkService>("DecorateWorkService[AAA]");
            if (workServiceE == null) throw new InvalidOperationException($"{nameof(workServiceE)}=null");

            // ResolveF
            var workServiceF = serviceProvider.ResolveNamed<WorkService>("DecorateWorkService[BBB]");
            if (workServiceF == null) throw new InvalidOperationException($"{nameof(workServiceF)}=null");

            // ResolveG
            var workServiceG = serviceProvider.ResolveNamed<WorkService>("DecorateWorkService[CCC]");
            if (workServiceG == null) throw new InvalidOperationException($"{nameof(workServiceG)}=null");

            // Execute
            Console.WriteLine(workServiceA.GetValue());
            Console.WriteLine(workServiceB.GetValue());
            Console.WriteLine(workServiceC.GetValue());
            Console.WriteLine(workServiceD.GetValue());
            Console.WriteLine(workServiceE.GetValue());
            Console.WriteLine(workServiceF.GetValue());
            Console.WriteLine(workServiceG.GetValue());
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
