using Autofac;

namespace MDP.Hosting.Lab
{
    public class DecorateWorkServiceFactory : ServiceFactory<WorkService, DecorateWorkService, DecorateWorkServiceFactory.Setting>
    {
        // Methods
        protected override DecorateWorkService CreateService(IComponentContext componentContext, Setting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // ComponentContext
            var lifetimeComponentContext = componentContext.Resolve<IComponentContext>();
            if (lifetimeComponentContext == null) throw new InvalidOperationException($"{nameof(lifetimeComponentContext)}=null");

            // WorkService
            var workService = lifetimeComponentContext.Resolve<WorkService>(setting.WorkService);
            if (workService == null) throw new InvalidOperationException($"{nameof(workService)}=null");

            // Create
            return new DecorateWorkService
            (
                setting.Message,
                workService
            );
        }


        // Class
        public class Setting
        {
            // Properties
            public string Message { get; set; } = string.Empty;

            public string WorkService { get; set; } = string.Empty;
        }
    }
}
