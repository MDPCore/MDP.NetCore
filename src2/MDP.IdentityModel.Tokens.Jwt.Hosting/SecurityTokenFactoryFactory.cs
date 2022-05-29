using Autofac;
using MDP.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.IdentityModel.Tokens.Jwt.Hosting
{
    public class SecurityTokenFactoryFactory : ServiceFactory<SecurityTokenFactory, SecurityTokenFactory, SecurityTokenSetting>
    {
        // Constructors
        public SecurityTokenFactoryFactory() 
        {
            // Default
            this.ServiceSingleton = true;
        }


        // Methods
        protected override SecurityTokenFactory CreateService(IComponentContext componentContext, SecurityTokenSetting setting)
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException($"{nameof(componentContext)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Create
            return new SecurityTokenFactory
            (
                setting
            );
        }
    }
}
