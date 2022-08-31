using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MDP.AspNetCore
{
    internal static class WebApplicationExtensions
    {
        // Methods
        public static WebApplication ConfigureDefault(this WebApplication host)
        {
            #region Contracts

            if (host == null) throw new ArgumentException($"{nameof(host)}=null");

            #endregion

            // Environment
            if (host.Environment.IsDevelopment() == false)
            {
                host.UseExceptionHandler("/Error");
            }
            else
            {
                host.UseWhen(context => context.Request.HasAccept(new List<string>() { "html" }) == false, applicationBuilder =>
                {
                    applicationBuilder.UseExceptionHandler("/Error");
                });
            }
            host.UseHook(HookMiddlewareDefaults.EnteringHook);

            // Swagger
            if (host.Environment.IsDevelopment() == true)
            {
                host.UseSwagger();
                host.UseSwaggerUI();
            }

            // Network 
            host.UseForwardedHeaders();

            // Security
            host.UseHsts();
            host.UseHttpsRedirection();            

            // StaticFile
            host.UseDefaultFiles();
            host.UseStaticFiles();

            // Routing
            host.UseRouting().UseHook(HookMiddlewareDefaults.RoutingHook);
            {
                // Cors
                host.UseCors();

                // Auth
                host.UseAuthentication();
                host.UseAuthorization();
            }
            host.MapControllers();
            host.MapDefaultControllerRoute();
            host.UseHook(HookMiddlewareDefaults.RoutedHook);

            // Return
            return host;
        }
    }
}
