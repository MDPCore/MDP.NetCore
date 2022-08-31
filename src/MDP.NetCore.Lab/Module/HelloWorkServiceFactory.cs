using Autofac;
using MDP.Hosting;

namespace MDP.NetCore.Lab
{
    public class HelloWorkServiceFactory : ServiceFactory<WorkService, HelloWorkService, HelloWorkServiceFactory.Setting>
    {
        // Methods
        protected override HelloWorkService CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Create
            return new HelloWorkService(
                setting.Message
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; } = string.Empty;
        }
    }
}
