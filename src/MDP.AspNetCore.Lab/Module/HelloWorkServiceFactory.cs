using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Lab
{
    public class HelloWorkServiceFactory : Factory<WorkService, HelloWorkService>
    {
        // Properties
        public string Message { get; set; }


        // Methods
        protected override HelloWorkService CreateService(IComponentContext componentContext)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // Create
            return new HelloWorkService(this.Message);
        }
    }
}
