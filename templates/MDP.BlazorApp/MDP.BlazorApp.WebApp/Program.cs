using MDP.BlazorApp.CoreApp;
using MDP.BlazorApp.CoreApp.Layout;

namespace MDP.BlazorApp.WebApp
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
