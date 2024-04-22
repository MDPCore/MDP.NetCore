using System;
using MDP.AspNetCore;
using Microsoft.AspNetCore.Components;

namespace MDP.BlazorCore.Web
{
    public static class Host
    {
        // Methods
        public static void Run<TApp>(string[] args) where TApp : ComponentBase
        {
            #region Contracts

            if (args == null) throw new ArgumentException($"{nameof(args)}=null");

            #endregion

            // ApplicationBuilder
            var applicationBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args).ConfigureMDP<TApp>();
            if (applicationBuilder == null) throw new InvalidOperationException($"{nameof(applicationBuilder)}=null");

            // Application
            var application = applicationBuilder.Build().ConfigureMDP();
            if (application == null) throw new InvalidOperationException($"{nameof(application)}=null");

            // Run
            application.Run();
        }
    }
}
