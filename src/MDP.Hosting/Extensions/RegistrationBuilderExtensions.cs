using Autofac;
using Autofac.Builder;
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
    public static class RegistrationBuilderExtensions
    {
        // Methods   
        public static IList<IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle>> SingleInstance<TService>(this IList<IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle>> registrationBuilderList)
            where TService : class
        {
            #region Contracts

            if (registrationBuilderList == null) throw new ArgumentException(nameof(registrationBuilderList));

            #endregion

            // RegistrationBuilderList
            foreach (var registrationBuilder in registrationBuilderList)
            {
                // SingleInstance
                registrationBuilder.SingleInstance();
            }

            // Return
            return registrationBuilderList;
        }
    }
}
