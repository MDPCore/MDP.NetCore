using MDP.BlazorCore.Lab;
using MDP.BlazorCore.Lab.Layout;

namespace MDP.BlazorCore.Web.Lab
{
    public class Program
    {
        // Methods
        public static void Main()
        {
            // Host
            MDP.BlazorCore.Web.Host.Run<App>(typeof(MainLayout));
        }
    }
}
