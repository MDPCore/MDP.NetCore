using MyLab.Module;
using System;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(WorkContext workContext)
        {
            #region Contracts

            if (workContext == null) throw new ArgumentException($"{nameof(workContext)}=null");

            #endregion

            // Display
            Console.WriteLine(workContext.GetValue());
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);            
        }
    }
}
