using MDP.AspNetCore.Authentication;
using MDP.AspNetCore.Authentication.External;
using MDP.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.WebApp
{
    public class Startup
    {
        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Config
            var issuer = "MDP.WebApp";
            var signKey = "12345678901234567890123456789012";

            // Authentication   
            services.AddAuthentication(options =>
            {
                // DefaultScheme
                options.DefaultScheme = PolicyAuthenticationDefaults.AuthenticationScheme;
            })
            .AddPolicy(options =>
            {
                // DefaultScheme
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // AuthenticateSchemePolicy
                options.AuthenticateSchemePolicy = context =>
                {
                    // JwtBearer
                    if (context.HasJwtBearer() == true) return JwtBearerDefaults.AuthenticationScheme;

                    // Default
                    return options.DefaultScheme;
                };
            })
            .AddJwtBearer(options =>
            {
                // Decode
                options.Issuer = issuer;
                options.SignKey = signKey;
            })
            .AddCookie(options =>
            {
                // Action
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = options.LoginPath;
            })
            .AddExternalCookie(options =>
            {
                // Action
                options.CallbackPath = new PathString("/Account/ExternalSignIn");
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                // Client
                options.ClientId = @"";
                options.ClientSecret = @"";
            })
            .AddFacebook(options =>
            {
                // Client
                options.ClientId = @"";
                options.ClientSecret = @"";
            });

            // SecurityTokenFactory
            services.AddSecurityTokenFactory(options =>
            {
                // Encode
                options.Issuer = issuer;
                options.SignKey = signKey;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Contracts

            if (app == null) throw new ArgumentException(nameof(app));
            if (env == null) throw new ArgumentException(nameof(env));

            #endregion

            // Development
            if (env.IsDevelopment() == true)
            {
                app.UseDeveloperExceptionPage();
            }

            // StaticFile
            app.UseStaticFiles();

            // Authentication
            app.UseAuthentication();

            // Routing            
            app.UseRouting();
            {

            }

            // Authorization
            app.UseAuthorization();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                // Default
                endpoints.MapControllerRoute
                (
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}"
                );
            });
        }
    }
}
