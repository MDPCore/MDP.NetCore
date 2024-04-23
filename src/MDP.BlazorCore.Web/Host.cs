using System;
using MDP.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace MDP.BlazorCore.Web
{
    public static class Host
    {
        // Methods
        public static void Run<TApp>(Type defaultLayout = null) where TApp : ComponentBase
        {
            // ApplicationBuilder
            var applicationBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder().ConfigureMDP<TApp>(defaultLayout);
            if (applicationBuilder == null) throw new InvalidOperationException($"{nameof(applicationBuilder)}=null");

            // Application
            var application = applicationBuilder.Build().ConfigureMDP();
            if (application == null) throw new InvalidOperationException($"{nameof(application)}=null");

            // Run
            application.Run();
        }
    }
}
