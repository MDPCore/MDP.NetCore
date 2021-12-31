using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public static class ContainerExtensions
    {
        // Methods   
        public static TService Resolve<TService>(this IComponentContext componentContext, string serviceName) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (string.IsNullOrEmpty(serviceName) == true) throw new ArgumentException(nameof(serviceName));

            #endregion

            // Resolve
            return componentContext.Resolve<TService>(new ServiceNameParameter(serviceName));
        }
    }
}
