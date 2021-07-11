using MDP.AspNetCore.Authentication.Policies;
using MDP.AspNetCore.Authentication.ExternalCookies;
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
using MDP.NetCore.Logging.Log4net;
using MDP.NetCore.Logging.NLog;

namespace MDP.WebApp
{
    public class Startup
    {
        // Config
        private static string _issuer = "MDP.WebApp";

        private static string _signKey = "12345678901234567890123456789012";


        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Kernel
            this.ConfigureLogger(services);
            this.ConfigureAuthentication(services);

            // Service
            this.AddSecurityTokenFactory(services, options =>
            {
                // Encode
                options.Issuer = _issuer;
                options.SignKey = _signKey;
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


        // Kernel
        private IServiceCollection ConfigureLogger(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Log4net
            services.AddLog4netLogger(options =>
            {
                options.Properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            });

            // NLog
            services.AddNLogLogger(options =>
            {
                options.Properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            });

            // Return
            return services;
        }

        private IServiceCollection ConfigureAuthentication(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

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
                options.Issuer = _issuer;
                options.SignKey = _signKey;
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

            // Return
            return services;
        }

        // Service        
        public IServiceCollection AddSecurityTokenFactory(IServiceCollection services, Action<SecurityTokenFactoryOptions> configureOptions = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // SecurityTokenFactory
            services.AddSecurityTokenFactory(options =>
            {
                // Encode
                options.Issuer = _issuer;
                options.SignKey = _signKey;
            });

            // Return
            return services;
        }
    }
}
