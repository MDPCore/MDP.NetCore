using MDP.BlazorApp.CoreApp;
using MDP.BlazorApp.CoreApp.Layout;

namespace MDP.BlazorApp.MauiApp
{
    public class MauiProgram
    {
        // Methods
        public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
        {
            // Host
            return MDP.BlazorCore.Maui.Host.CreateMauiApp<MauiProgram>(typeof(MainLayout));
        }
    }
}
