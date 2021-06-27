using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using CLK.Autofac;
using MDP.Hosting;
using MDP.Hosting.Options;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlCommandFactoryModule : MDP.Hosting.Module
    {
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public SqlCommandFactoryModule(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Default
            _configuration = configuration;
        }


        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // Service
            container.RegisterType<SqlClientFactory>().As<SqlClientFactory>().SingleInstance();
            container.RegisterType<DefaultSqlCommandFactory>().As<SqlCommandFactory>().SingleInstance();

            // Options
            var sqlCommandConfigList = _configuration.GetSection("MDP.Data.SqlClient.SqlCommand")?.GetChildren();
            if (sqlCommandConfigList == null) sqlCommandConfigList = new List<IConfigurationSection>();
            foreach (var sqlCommandConfig in sqlCommandConfigList)
            {
                container.Configure<SqlCommandOption>(sqlCommandConfig.Key, () => sqlCommandConfig);
            }  
        }
    }
}