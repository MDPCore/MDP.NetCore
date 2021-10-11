using MDP.AspNetCore.Authentication.Policies;
using MDP.AspNetCore.Authentication.ExternalCookies;
using MDP.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.NetCore.Logging.NLog;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using MDP.AspNetCore.Authentication.Line;
using Microsoft.AspNetCore.HttpOverrides;

namespace BiosWorkshop.CRM.WebPlatform
{
    public class Startup
    {
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public Startup(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Default
            _configuration = configuration;
        }


        // Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Kernel
            this.ConfigureLogger(services);
            this.ConfigureAuthentication(services);
            this.ConfigureForwardedHeaders(services);

            // Service
            this.AddSecurityTokenFactory(services);
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

            // Network
            app.UseForwardedHeaders();

            // StaticFile
            app.UseDefaultFiles();
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
                // Options
                options.Issuer = _configuration["Authentication:JwtBearer:Issuer"];
                options.SignKey = _configuration["Authentication:JwtBearer:SignKey"];
            })
            .AddCookie(options =>
            {
                // Options
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = options.LoginPath;
            })
            .AddExternalCookie(options =>
            {
                // Options
                //options.RegisterPath = new PathString("/registration.html");
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddLine(options =>
            {
                // Options
                options.ClientId = _configuration["Authentication:Line:ClientId"];
                options.ClientSecret = _configuration["Authentication:Line:ClientSecret"];
                options.ExternalSignIn();
            })
            .AddLiff(options =>
            {
                // Options
                options.ClientId = _configuration["Authentication:Line:ClientId"];
                options.ClientSecret = _configuration["Authentication:Line:ClientSecret"];
                options.ExternalSignIn();
            });

            // Return
            return services;
        }

        private IServiceCollection ConfigureForwardedHeaders(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Default
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // Config
            services.Configure<ForwardedHeadersOptions>
            (
                _configuration.GetSection("Http:ForwardedHeaders")
            );
          
            // Return
            return services;
        }

        // Service        
        public IServiceCollection AddSecurityTokenFactory(IServiceCollection services)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // SecurityTokenFactory
            services.AddSecurityTokenFactory(options =>
            {
                // Encode
                options.Issuer = _configuration["Authentication:JwtBearer:Issuer"];
                options.SignKey = _configuration["Authentication:JwtBearer:SignKey"];
            });

            // Return
            return services;
        }
    }
}
