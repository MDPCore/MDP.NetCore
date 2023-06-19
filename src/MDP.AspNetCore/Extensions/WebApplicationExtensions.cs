using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

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
                webApplication.UseWhen(context => context.Request.HasAccept("html") == false, applicationBuilder =>
                {
                    applicationBuilder.UseExceptionHandler("/Error");
                });
            }
            webApplication.UseHook(HookMiddlewareDefaults.EnteringHook);

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
