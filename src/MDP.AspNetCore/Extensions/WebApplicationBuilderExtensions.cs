using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static partial class WebApplicationBuilderExtensions
    {
        // Methods
        public static WebApplicationBuilder ConfigureDefault(this WebApplicationBuilder webApplicationBuilder)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");

            #endregion

            // RegisterModule
            webApplicationBuilder.RegisterModule();

            // MvcBuilder
            var mvcBuilder = webApplicationBuilder.Services.AddMvc();
            {
                // Default
                mvcBuilder.AddMvcDefaults();
                mvcBuilder.AddMvcCors(webApplicationBuilder.Configuration);
                mvcBuilder.AddMvcForwardedHeaders(webApplicationBuilder.Configuration);
            }

            // Return
            return webApplicationBuilder;
        }


        // MvcBuilder
        private static void AddMvcDefaults(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // MvcOptions
            mvcBuilder.AddMvcOptions((options) =>
            {
                // NotAcceptable
                options.ReturnHttpNotAcceptable = false;

                // AcceptHeader
                options.RespectBrowserAcceptHeader = true;

                // OutputFormatters:Null
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                options.OutputFormatters.Insert(0, new NullContentOutputFormatter());
            });

            // RazorOptions
            mvcBuilder.AddRazorOptions((options) =>
            {
                // ViewLocation
                options.ViewLocationFormats.Add("/Views/{0}.cshtml");

                // AreaViewLocation
                options.AreaViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/{2}/{0}.cshtml");
            });

            // HttpContext
            mvcBuilder.Services.AddHttpContextAccessor();

            // HtmlEncoder
            mvcBuilder.Services.AddSingleton<HtmlEncoder>
            (
                HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            );

            // JsonOptions
            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
                options.JsonSerializerOptions.Converters.Add(new DateTimeISO8601Converter());
            });

            // LoggerOptions
            mvcBuilder.Services.AddLogging(builder =>
            {
                // Filter
                builder.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
            });
        }

        private static void AddMvcCors(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // Cors
            mvcBuilder.Services.AddCors(option =>
            {
                option.AddDefaultPolicy(corsBuilder =>
                {
                    corsBuilder.AllowAnyOrigin();
                    corsBuilder.AllowAnyHeader();
                    corsBuilder.AllowAnyMethod();
                });
            });
            mvcBuilder.Services.Configure<CorsOptions>(configuration.GetSection("Http:Cors"));
        }

        private static void AddMvcForwardedHeaders(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // ForwardedHeaders
            mvcBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            mvcBuilder.Services.Configure<ForwardedHeadersOptions>(configuration.GetSection("Http:ForwardedHeaders"));
        }


        // Class
        private class NullContentOutputFormatter : IOutputFormatter
        {
            // Methods
            public bool CanWriteResult(OutputFormatterCanWriteContext context)
            {
                #region Contracts

                if (context == null) throw new ArgumentException($"{nameof(context)}=null");

                #endregion

                // Void
                if (context.ObjectType == typeof(void) || context.ObjectType == typeof(Task))
                {
                    return true;
                }

                // Null
                if (context.Object == null)
                {
                    return true;
                }

                // Return
                return false;
            }

            public Task WriteAsync(OutputFormatterWriteContext context)
            {
                #region Contracts

                if (context == null) throw new ArgumentException($"{nameof(context)}=null");

                #endregion

                // Content
                context.HttpContext.Response.ContentLength = 0;

                // Return
                return Task.CompletedTask;
            }
        }

        private class DateTimeISO8601Converter : JsonConverter<DateTime>
        {
            // Methods
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                #region Contracts

                if (typeToConvert == null) throw new ArgumentException($"{nameof(typeToConvert)}=null");
                if (options == null) throw new ArgumentException($"{nameof(options)}=null");

                #endregion

                // DataTimeString
                var dataTimeString = reader.GetString();
                if (string.IsNullOrEmpty(dataTimeString) == true) throw new InvalidOperationException($"{nameof(dataTimeString)}=null");

                // DataTime
                var dateTime = DateTime.Parse(dataTimeString);
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }

                // Parse
                return dateTime;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                #region Contracts

                if (writer == null) throw new ArgumentException($"{nameof(writer)}=null");
                if (options == null) throw new ArgumentException($"{nameof(options)}=null");

                #endregion

                // DataTime
                var dateTime = value;
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }

                // DataTimeString
                var dataTimeString = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ssK", CultureInfo.InvariantCulture);
                if (string.IsNullOrEmpty(dataTimeString) == true) throw new InvalidOperationException($"{nameof(dataTimeString)}=null");

                // Write
                writer.WriteStringValue(dataTimeString);
            }
        }
    }

    public static partial class WebApplicationBuilderExtensions
    {
        // Methods
        public static WebApplicationBuilder RegisterModule(this WebApplicationBuilder webApplicationBuilder)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");

            #endregion

            // HostBuilder
            var hostBuilder = webApplicationBuilder.Host;
            {
                // RegisterModule
                hostBuilder.RegisterModule();
            }

            // WebApplicationBuilder
            {
                // FactoryRegisterContext
                var factoryRegisterContext = new FactoryRegisterContext<WebApplicationBuilder>();
                {
                    // RegisterModule
                    factoryRegisterContext.RegisterModule(webApplicationBuilder, webApplicationBuilder.Configuration);
                }

                // MvcBuilder
                var mvcBuilder = webApplicationBuilder.Services.AddControllersWithViews();
                {
                    // RegisterModule
                    mvcBuilder.RegisterModule();
                }
            }            

            // Return
            return webApplicationBuilder;
        }
    }
}