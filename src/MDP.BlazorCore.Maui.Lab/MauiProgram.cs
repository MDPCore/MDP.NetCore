using MDP.BlazorCore.Lab;
using MDP.BlazorCore.Lab.Layout;

namespace MDP.BlazorCore.Maui.Lab
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
