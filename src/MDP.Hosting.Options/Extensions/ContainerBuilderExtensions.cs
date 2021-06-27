using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MDP.Hosting.Options
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, Func<T1, IEnumerable<IConfiguration>> configFactory)
            where TOptions : class
            where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.Register<T1, IConfigureOptions<TOptions>>((t1) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>("", configFactory(t1).First());
            })
            .SingleInstance();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, Func<IConfiguration> configFactory)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, Func<T1, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, Func<T1, T2, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, Func<T1, T2, T3, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }


        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, string name, Func<IConfiguration> configFactory)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.Register<IConfigureOptions<TOptions>>((t1) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory());
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, string name, Func<T1, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.Register<T1, IConfigureOptions<TOptions>>((t1) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1));
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, string name, Func<T1, T2, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.Register<T1, T2, IConfigureOptions<TOptions>>((t1, t2) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1, t2));
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Func<T1, T2, T3, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.Register<T1, T2, T3, IConfigureOptions<TOptions>>((t1, t2, t3) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1, t2, t3));
            })
            .SingleInstance();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }


        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<IConfigureOptions<TOptions>>(() =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions>(name, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, string name, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, IConfigureOptions<TOptions>>((t1) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1>(name, t1, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, string name, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, T2, IConfigureOptions<TOptions>>((t1, t2) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1, T2>(name, t1, t2, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, T2, T3, IConfigureOptions<TOptions>>((t1, t2, t3) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1, T2, T3>(name, t1, t2, t3, configureOptions);
            })
            .SingleInstance();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions>(this ContainerBuilder container, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1>(this ContainerBuilder container, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2>(this ContainerBuilder container, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2, T3>(this ContainerBuilder container, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }


        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions>(this ContainerBuilder container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<IPostConfigureOptions<TOptions>>(() =>
            {
                // Create
                return new PostConfigureOptions<TOptions>(name, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1>(this ContainerBuilder container, string name, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, IPostConfigureOptions<TOptions>>((t1) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1>(name, t1, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2>(this ContainerBuilder container, string name, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, T2, IPostConfigureOptions<TOptions>>((t1, t2) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1, T2>(name, t1, t2, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.Register<T1, T2, T3, IPostConfigureOptions<TOptions>>((t1, t2, t3) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1, T2, T3>(name, t1, t2, t3, configureOptions);
            })
            .SingleInstance();
        }
    }
}