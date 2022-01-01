using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting.Lab
{
    public class HelloWorkServiceFactory : Factory<WorkService, HelloWorkService, HelloWorkServiceFactory.Setting>
    {
        // Methods
        protected override HelloWorkService CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Create
            return new HelloWorkService(setting.Message);
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; } = "Hello World";
        }
    }
}
