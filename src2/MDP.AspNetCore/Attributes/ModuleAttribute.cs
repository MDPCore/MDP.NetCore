using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MDP.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : ApiControllerAttribute, IRouteValueProvider, IRouteTemplateProvider
    {
        // Fields
        private readonly AreaAttribute _areaAttribute;

        private readonly RouteAttribute _routeAttribute;


        // Constructors
        public ModuleAttribute(string moduleName, string routeTemplate = @"{area}/{controller=Home}/{action=Index}")
        {
            #region Contracts

            if (string.IsNullOrEmpty(moduleName) == true) throw new ArgumentException($"{nameof(moduleName)}=null");

            #endregion

            // Default
            _areaAttribute = new AreaAttribute(moduleName);
            _routeAttribute = new RouteAttribute(routeTemplate);
        }


        // AreaAttribute
        string IRouteValueProvider.RouteKey => _areaAttribute.RouteKey;

        string IRouteValueProvider.RouteValue => _areaAttribute.RouteValue;

        // RouteAttribute
        string IRouteTemplateProvider.Template => _routeAttribute.Template;

        int? IRouteTemplateProvider.Order => _routeAttribute.Order;

        string IRouteTemplateProvider.Name => _routeAttribute.Name ?? String.Empty;
    }
}
