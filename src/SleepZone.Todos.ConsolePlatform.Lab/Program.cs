using Autofac;
using MDP.Module01;
using Microsoft.Extensions.Hosting;
using System;

namespace SleepZone.Todos.ConsolePlatform.Lab
{
    public class Program
    {
        // Methods
        public static void Run(SettingContext settingContext)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // Message
            var message = settingContext.GetValue();
            if (string.IsNullOrEmpty(message) == true) throw new InvalidOperationException($"{nameof(message)}=null");

            // Display
            Console.WriteLine(message);
        }

        public static void Main(string[] args)
        {
            // Host
            SleepZone.Todos.ConsolePlatform.Host.Run<Program>(args);
        }
    }
}
