using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;

namespace MDP.Caching.Memory.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // MemoryCache
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Loop
            for (var i = 0; i < 11; i++)
            {
                // GetValue
                var value = memoryCache.GetValue("Time", () =>
                {
                    // Return
                    return DateTime.Now;
                }
                , TimeSpan.FromSeconds(5));

                // Display
                Console.WriteLine(value);

                // Delay
                Thread.Sleep(1000);     
            }
        }
    }
}
