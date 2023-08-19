using Microsoft.Extensions.Hosting;

namespace MDP.Distributions.Lab
{
    public class Program
    {
        // Methods
        public static void Run(DistributeContext distributeContext)
        {
            #region Contracts

            if (distributeContext == null) throw new ArgumentException($"{nameof(distributeContext)}=null");

            #endregion

            // Variables
            var syncRoot = new object();
            var count = 0;
            var stopWatch = new System.Diagnostics.Stopwatch();

            // None
            Console.WriteLine($"None start");
            count = 0;
            stopWatch.Restart();
            Parallel.For(0, 1000, i =>
            {
                // Sync
                Thread.Sleep(100);

                // Add
                count++;
            });
            stopWatch.Stop();
            Console.WriteLine($"count={count}, time={stopWatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"None end\n");

            // Lock
            Console.WriteLine($"Lock start");
            count = 0;
            stopWatch.Restart();
            Parallel.For(0, 1000, i =>
            {
                // Sleep
                Thread.Sleep(100);

                // Sync
                lock (syncRoot)
                {          
                    // Add
                    count++;
                }
            });
            stopWatch.Stop();
            Console.WriteLine($"count={count}, time={stopWatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Lock end\n");

            // DistributedLock
            Console.WriteLine($"DistributedLock start");
            count = 0;
            stopWatch.Restart();
            Parallel.For(0, 1000, i =>
            {
                // Sleep
                Thread.Sleep(100);

                // Sync
                using (distributeContext.Lock("Clark001"))
                {
                    // Add
                    count++;
                }
            });
            stopWatch.Stop();
            Console.WriteLine($"count={count}, time={stopWatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"DistributedLock end\n");
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}
