using Autofac;
using Autofac.Extensions.DependencyInjection;
using CLK.Diagnostics;
using MDP.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MDP.AspNetCore
{
    internal static class WebApplicationBuilderExtensions
    {
        // Methods
        public static WebApplicationBuilder ConfigureDefault(this WebApplicationBuilder hostBuilder, Action<WebApplicationBuilder>? configureAction = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Service
            hostBuilder.AddTracer();
            hostBuilder.AddOptions();
            hostBuilder.AddHttpClient();

            // Hosting
            hostBuilder.AddAutofac();
            hostBuilder.AddModule();

            // AspNetCore
            hostBuilder.AddMvc();
            hostBuilder.AddSwagger();

            // Action
            configureAction?.Invoke(hostBuilder);

            // Return
            return hostBuilder;
        }


        // Service
        private static void AddTracer(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.Host.ConfigureServices((context, services) =>
            {
                // Tracer
                services.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));
            });
        }

        private static void AddOptions(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.Host.ConfigureServices((context, services) =>
            {
                // Options
                services.AddOptions();
            });
        }

        private static void AddHttpClient(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.Host.ConfigureServices((context, services) =>
            {
                // HttpClientFactory
                services.AddHttpClient();

                // Func<HttpClientFactory>
                services.AddTransient<Func<IHttpClientFactory>>(serviceProvider => () =>
                {
                    return serviceProvider.GetService<IHttpClientFactory>()!;
                });
            });
        }

        public static void AddProgramService<TProgram>(this WebApplicationBuilder hostBuilder)
            where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.Host.ConfigureServices((context, services) =>
            {
                // Program
                services.TryAddTransient<TProgram, TProgram>();

                // ProgramService
                services.Add(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });
        }

        // Hosting
        private static void AddAutofac(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Autofac
            hostBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        private static void AddModule(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Configuration
            hostBuilder.Host.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                configurationBuilder.RegisterModule(hostContext.HostingEnvironment);
            });

            // Container
            hostBuilder.Host.ConfigureContainer<Autofac.ContainerBuilder>((hostContext, containerBuilder) =>
            {
                containerBuilder.RegisterModule(hostContext.Configuration);
            });

            // Host
            hostBuilder.Host.ConfigureServices((context, services) =>
            {
                MDP.Hosting.ServiceBuilder.RegisterModule(Tuple.Create(context, services), context.Configuration);
            });
            MDP.Hosting.ServiceBuilder.RegisterModule(hostBuilder, hostBuilder.Configuration);
        }

        // AspNetCore
        private static void AddMvc(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // HttpContext
            hostBuilder.Services.AddHttpContextAccessor();

            // Logger
            hostBuilder.Services.AddLogging(builder =>
            {
                // Filter
                builder.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
            });

            // Mvc
            var mvcBuilder = hostBuilder.Services.AddMvc();
            {
                mvcBuilder.AddMvcPart();
                mvcBuilder.AddMvcAsset();
            }

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

            // JsonOptions
            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
                options.JsonSerializerOptions.Converters.Add(new DateTimeISO8601Converter());
            });

            // HtmlEncoder
            hostBuilder.Services.AddSingleton<HtmlEncoder>
            (
                HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            );

            // Cors
            hostBuilder.Services.AddCors(option =>
            {
                option.AddDefaultPolicy(corsBuilder =>
                {
                    corsBuilder.AllowAnyOrigin();
                    corsBuilder.AllowAnyHeader();
                    corsBuilder.AllowAnyMethod();
                });
            });
            hostBuilder.Services.Configure<CorsOptions>(hostBuilder.Configuration.GetSection("Http:Cors"));

            // ForwardedHeaders
            hostBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            hostBuilder.Services.Configure<ForwardedHeadersOptions>(hostBuilder.Configuration.GetSection("Http:ForwardedHeaders"));
        }

        private static void AddMvcPart(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"MDP.AspNetCore.dll|*.Services.dll|*.Services.Views.dll")
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // RegisteredAssembly
            var registeredAssemblyList = new List<Assembly>();
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));
            registeredAssemblyList.AddRange(mvcBuilder.PartManager.ApplicationParts.OfType<CompiledRazorAssemblyPart>().Select(assemblyPart => assemblyPart.Assembly));

            // ApplicationPart
            foreach (var moduleAssembly in moduleAssemblyList)
            {
                if (registeredAssemblyList.Contains(moduleAssembly) == false)
                {
                    mvcBuilder.AddApplicationPart(moduleAssembly);
                }
            }
        }

        private static void AddMvcAsset(this IMvcBuilder mvcBuilder, string moduleAssemblyFileName = @"MDP.AspNetCore.dll|*.Services.dll|*.Services.Views.dll")
        {
            #region Contracts

            if (mvcBuilder == null) throw new ArgumentException(nameof(mvcBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
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
                if (assetAssemblyList.Contains(moduleAssembly) == true)
                {
                    assetAssemblyList.Remove(moduleAssembly);
                }
                assetAssemblyList.Add(moduleAssembly);
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

        private static void AddSwagger(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // ApiExplorer
            hostBuilder.Services.AddEndpointsApiExplorer();

            // Swagger
            hostBuilder.Services.AddSwaggerGen(setupAction =>
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
                    if(commentFile!=null) setupAction.IncludeXmlComments(commentFile.FullName);
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
