using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace MDP.AspNetCore
{
    internal static class WebApplicationExtensions
    {
        // Methods
        public static WebApplication ConfigureMDP(this WebApplication webApplication)
        {
            #region Contracts

            if (webApplication == null) throw new ArgumentException($"{nameof(webApplication)}=null");

            #endregion

            // ExceptionHandler
            if (webApplication.Environment.IsDevelopment() == false)
            {
                webApplication.UseProblemDetails();
            }
            webApplication.UseHook("ExceptionHandler");

            // Network 
            webApplication.UsePathBase();
            webApplication.UsePathDefault();
            webApplication.UseForwardedHeaders();

            // Security
            webApplication.UseHsts();
            webApplication.UseHttpsRedirection();            

            // StaticFile
            webApplication.UseDefaultFiles();
            webApplication.UseStaticFiles();

            // Routing
            webApplication.UseRouting().UseHook("Routing");

            // Auth
            webApplication.UseAuthentication();
            webApplication.UseAuthorization();

            // MVC
            webApplication.MapControllers();
            webApplication.MapDefaultControllerRoute();

            // Return
            return webApplication;
        }
    }
}
