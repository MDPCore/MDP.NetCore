using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Hosting;

namespace MDP.Data.SqlClient.Hosting
{
    public class SqlClientFactoryOptions
    {
        // Properties
        public Dictionary<string, string> ConnectionStrings { get; set; }
    }
}
