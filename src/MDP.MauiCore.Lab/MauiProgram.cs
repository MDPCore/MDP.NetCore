using Microsoft.Extensions.Logging;
using MDP.MauiCore;

namespace MDP.MauiCore.Lab
{
    public class MauiProgram
    {
        // Methods
        public static MauiApp CreateMauiApp()
        {
            // Host
            return MDP.MauiCore.Host.CreateMauiApp<App>();
        }
    }
}
