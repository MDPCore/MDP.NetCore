using MDP.NetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

            // HostBuilder
            var hostBuilder = webApplicationBuilder.Host;
            {
                // Default
                hostBuilder.ConfigureDefault();
            }

            // WebApplicationBuilder
            {
                // MvcBuilder
                var mvcBuilder = webApplicationBuilder.Services.AddMvc().ConfigureDefault();
                {
                    mvcBuilder.AddMvcPart();
                    mvcBuilder.AddMvcAsset();
                    mvcBuilder.AddMvcCors(webApplicationBuilder.Configuration);
                    mvcBuilder.AddMvcSwagger(webApplicationBuilder.Configuration);
                    mvcBuilder.AddMvcForwardedHeaders(webApplicationBuilder.Configuration);
                }

                // RegisterContext
                var registerContext = new FactoryRegisterContext<WebApplicationBuilder>();
                {
                    // Module
                    registerContext.RegisterModule(webApplicationBuilder, webApplicationBuilder.Configuration);
                }
            }

            // Return
            return webApplicationBuilder;
        }


        // MvcBuilder
        private static IMvcBuilder ConfigureDefault(this IMvcBuilder mvcBuilder)
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

            // Return
            return mvcBuilder;
        }

        private static void AddMvcPart(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.FindAllAssembly();
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // RegisteredAssembly
            var registeredAssemblyList = new List<Assembly>();
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<CompiledRazorAssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));

            // PartAssembly
            var partAssemblyList = new List<Assembly>();
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (registeredAssemblyList.Contains(moduleAssembly) == false)
                {
                    partAssemblyList.Add(moduleAssembly);
                }
            }

            // ApplicationPart
            foreach (var partAssembly in partAssemblyList)
            {
                mvcBuilder.AddApplicationPart(partAssembly);
            }
        }

        private static void AddMvcAsset(this IMvcBuilder mvcBuilder)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.FindAllAssembly();
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // RegisteredAssembly
            var registeredAssemblyList = new List<Assembly>();
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<CompiledRazorAssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));

            // AssetAssembly
            var assetAssemblyList = new List<Assembly>();
            foreach (var registeredAssembly in registeredAssemblyList)
            {
                if (assetAssemblyList.Contains(registeredAssembly) == false)
                {
                    assetAssemblyList.Add(registeredAssembly);
                }
            }
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (assetAssemblyList.Contains(moduleAssembly) == false)
                {
                    assetAssemblyList.Add(moduleAssembly);
                }
            }

            // FileProviderList
            var fileProviderList = new List<IFileProvider>();
            foreach (var assetAssembly in assetAssemblyList)
            {
                // FileProvider
                IFileProvider? fileProvider = null;
                try
                {
                    fileProvider = new ManifestEmbeddedFileProvider(assetAssembly, @"wwwroot");
                }
                catch
                {
                    fileProvider = null;
                }

                // Add
                if (fileProvider != null)
                {
                    fileProviderList.Add(fileProvider);
                }
            }

            // StaticFileOptions
            mvcBuilder.Services.AddOptions<StaticFileOptions>().Configure<IWebHostEnvironment>((options, hostEnvironment) =>
            {
                // FileProvider
                if (hostEnvironment.WebRootFileProvider != null)
                {
                    fileProviderList.Insert(0, hostEnvironment.WebRootFileProvider);
                }

                // Attach
                options.FileProvider = new CompositeFileProvider
                (
                    fileProviderList
                );
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

        private static void AddMvcSwagger(this IMvcBuilder mvcBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException($"{nameof(mvcBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // ApiExplorer
            mvcBuilder.Services.AddEndpointsApiExplorer();

            // Swagger
            mvcBuilder.Services.AddSwaggerGen(setupAction =>
            {
                // IncludeXmlComments
                {
                    // CommentFileName
                    var commentFileName = Assembly.GetEntryAssembly()?.GetName().Name + ".xml";
                    if (string.IsNullOrEmpty(commentFileName) == true) throw new InvalidOperationException($"{nameof(commentFileName)}=null");

                    // CommentFileName
                    var commentFileList = CLK.IO.File.GetAllFile(commentFileName);
                    if (commentFileList == null) throw new InvalidOperationException($"{nameof(commentFileList)}=null");

                    // Include
                    var commentFile = commentFileList.FirstOrDefault();
                    if (commentFile != null) setupAction.IncludeXmlComments(commentFile.FullName);
                }

                // TagAction
                setupAction.TagActionsBy(apiDescription =>
                {
                    // ActionDescriptor
                    var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                    if (actionDescriptor == null) return new[] { "Unknown" };

                    // Area
                    if (actionDescriptor.RouteValues.ContainsKey("area") == true)
                    {
                        var areaName = actionDescriptor.RouteValues["area"];
                        if (string.IsNullOrEmpty(areaName) == false) return new[] { areaName };
                    }

                    // Non-Area
                    return new[] { actionDescriptor.ControllerName };
                });
            });
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
}