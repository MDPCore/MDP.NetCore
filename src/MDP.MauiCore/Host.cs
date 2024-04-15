using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.MauiCore
{
    public static class Host
    {
        // Methods
        public static MauiApp CreateMauiApp<TApp>() where TApp : class, IApplication
        {
            // ApplicationBuilder
            var applicationBuilder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder().ConfigureMDP<TApp>();
            if (applicationBuilder == null) throw new InvalidOperationException($"{nameof(applicationBuilder)}=null");

            // Application
            var application = applicationBuilder.Build();
            if (application == null) throw new InvalidOperationException($"{nameof(application)}=null");

            // Return
            return application;
        }
    }
}
