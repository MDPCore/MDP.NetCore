using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.Threading.Tasks.Lab
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // ManualTask
            var manualTask = new ManualTask<int>();

            // 模擬異步操作
            Task.Run(() =>
            {
                Console.WriteLine("模擬工作開始...");
                Thread.Sleep(1000);
                Console.WriteLine("模擬工作完成，設置結果。");
                manualTask.TrySetResult(123);
                manualTask.TrySetResult(456);
                //manualTask.TrySetException(new Exception("AAAAA"));
                //manualTask.TrySetException(new Exception("BBBBB"));
                //manualTask.Cancel();
                //manualTask.Cancel();
            });

            // 等待結果
            using (manualTask)
            {
                try
                {
                    var result = await manualTask.WaitAsync(TimeSpan.FromMilliseconds(2000));
                    Console.WriteLine($"結果：{result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Message: {ex.Message}");
                }

                try
                {
                    var result = await manualTask.WaitAsync(TimeSpan.FromMilliseconds(2000));
                    Console.WriteLine($"結果：{result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Message: {ex.Message}");
                }
            }

            // End
            Console.WriteLine("End...");
            Console.ReadLine();
        }
    }
}
