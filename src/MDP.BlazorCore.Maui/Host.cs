using Microsoft.Maui.Hosting;
using System;

namespace MDP.BlazorCore.Maui
{
    public static class Host
    {
        // Methods
        public static MauiApp CreateMauiApp<TProgram>(Type defaultLayout = null) where TProgram : class
        {
            // ApplicationBuilder
            var applicationBuilder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder().ConfigureMDP<TProgram>(defaultLayout);
            if (applicationBuilder == null) throw new InvalidOperationException($"{nameof(applicationBuilder)}=null");

            // Application
            var application = applicationBuilder.Build();
            if (application == null) throw new InvalidOperationException($"{nameof(application)}=null");

            // Return
            return application;
        }
    }
}
