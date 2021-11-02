using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using MDP.Hosting;
using CLK.Data.SqlClient;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlClientFactoryModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // SqlClientFactory
            containerBuilder.RegisterService<SqlClientFactory>().SingleInstance();
            containerBuilder.RegisterFactory<SqlClientFactory, SqlClientFactoryFactory>();
        }
    }
}