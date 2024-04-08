using Microsoft.Extensions.Logging;

namespace MDP.Configuration.Maui.Lab
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // IsDevelopment
            var isDevelopment = false;
#if DEBUG
            isDevelopment = true;
#else
            isDevelopment = false;
#endif

            // EnvironmentName
            var environmentName = "Production";
            //var environmentName = "Staging";
            //var environmentName = "Development";

            // ExecutingAssembly
            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (executingAssembly == null) throw new InvalidDataException($"{nameof(executingAssembly)}=null");

            // applicationBuilder
            var applicationBuilder = MauiApp.CreateBuilder();
            {
                // Debug
                if (isDevelopment == true)
                {
                    applicationBuilder.Services.AddBlazorWebViewDeveloperTools();
                    applicationBuilder.Logging.AddDebug();
                }

                // MauiApp
                applicationBuilder.UseMauiApp<App>();

                // Font
                applicationBuilder.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

                // Blazor
                applicationBuilder.Services.AddMauiBlazorWebView();

                // Configuration
                var configurationBuilder = applicationBuilder.Configuration;
                {
                    // ConfigurationRegister
                    ConfigurationRegister.RegisterModule(configurationBuilder, new MDP.Configuration.EmbeddedConfigurationProvider(executingAssembly, environmentName));
                }
            }

            // Application
            var application = applicationBuilder.Build();
            {


            }

            // Return
            return application;
        }
    }
}
