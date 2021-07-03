using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using CLK.Autofac;
using MDP.Hosting;
using CLK.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlClientFactoryModule : MDP.Hosting.Module
    {
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public SqlClientFactoryModule(IConfiguration configuration)
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

            // SqlClientFactory
            container.RegisterServiceType<SqlClientFactory, SqlClientFactory, SqlClientFactoryFactory, SqlClientFactoryOptions>(_configuration, (builder) => builder.SingleInstance());
        }
    }
}