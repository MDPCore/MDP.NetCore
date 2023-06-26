using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MDP.DevKit.LineMessaging.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.AspNetCore.Host.Run(args);
        }
    }
}
