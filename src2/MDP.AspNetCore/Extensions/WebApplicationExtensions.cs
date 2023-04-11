using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MDP.AspNetCore
{
    internal static class WebApplicationExtensions
    {
        // Methods
        public static WebApplication ConfigureDefault(this WebApplication webApplication)
        {
            #region Contracts

            if (webApplication == null) throw new ArgumentException($"{nameof(webApplication)}=null");

            #endregion

            // Environment
            if (webApplication.Environment.IsDevelopment() == false)
            {
                webApplication.UseExceptionHandler("/Error");
            }
            else
            {
                webApplication.UseWhen(context => context.Request.HasAccept(new List<string>() { "html" }) == false, applicationBuilder =>
                {
                    applicationBuilder.UseExceptionHandler("/Error");
                });
            }
            webApplication.UseHook(HookMiddlewareDefaults.EnteringHook);

            // Swagger
            if (webApplication.Environment.IsDevelopment() == true)
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI(options => { options.DefaultModelsExpandDepth(-1); });
                webApplication.UseSwaggerDefaults();
            }

            // Network 
            webApplication.UsePathBase();
            webApplication.UseForwardedHeaders();

            // Security
            webApplication.UseHsts();
            webApplication.UseHttpsRedirection();            

            // StaticFile
            webApplication.UseDefaultFiles();
            webApplication.UseStaticFiles();

            // Routing
            webApplication.UseRouting().UseHook(HookMiddlewareDefaults.RoutingHook);
            {
                // Network
                webApplication.UseCors();
                webApplication.UsePathDefault();

                // Auth
                webApplication.UseAuthentication();
                webApplication.UseAuthorization();
            }
            webApplication.MapControllers();
            webApplication.MapDefaultControllerRoute();
            webApplication.UseHook(HookMiddlewareDefaults.RoutedHook);

            // Return
            return webApplication;
        }
    }
}
