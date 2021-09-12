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
using MDP.NetCore.Logging.Log4net;
using MDP.NetCore.Logging.NLog;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using MDP.AspNetCore.Authentication.Line;
using MDP.AspNetCore.Authentication.GitHub;
using Microsoft.AspNetCore.HttpOverrides;

namespace MDP.WebApp
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
            .AddJwtBearer(options =>
            {
                // Decode
                options.Issuer = _configuration["Authentication:JwtBearer:Issuer"];
                options.SignKey = _configuration["Authentication:JwtBearer:SignKey"];
            }) 
            .AddGoogle(options =>
            {
                // Client
                options.ClientId = _configuration["Authentication:Google:ClientId"];
                options.ClientSecret = _configuration["Authentication:Google:ClientSecret"];
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddFacebook(options =>
            {
                // Client
                options.ClientId = _configuration["Authentication:Facebook:ClientId"];
                options.ClientSecret = _configuration["Authentication:Facebook:ClientSecret"];
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddLine(options =>
            {
                // Client
                options.ClientId = _configuration["Authentication:Line:ClientId"];
                options.ClientSecret = _configuration["Authentication:Line:ClientSecret"];
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddLiff(options =>
            {
                // Client
                options.ClientId = _configuration["Authentication:Line:ClientId"];
                options.ClientSecret = _configuration["Authentication:Line:ClientSecret"];
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddGitHub(options => 
            {
                // Client
                options.ClientId = _configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = _configuration["Authentication:GitHub:ClientSecret"];
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
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
