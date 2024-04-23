using MDP.BlazorCore.Lab.Layout;
using MDP.BlazorCore.Web.Lab.Components;

namespace MDP.BlazorCore.Web.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.BlazorCore.Web.Host.Run<App>(typeof(MainLayout));
        }
    }
}
