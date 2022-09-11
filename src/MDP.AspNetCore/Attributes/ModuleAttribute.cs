using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MDP.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : ApiControllerAttribute, IRouteValueProvider, IRouteTemplateProvider
    {
        // Fields
        private readonly RouteValueAttribute _areaAttribute;

        private readonly RouteAttribute _routeAttribute;


        // Constructors
        public ModuleAttribute(string? moduleName = null)
        {
            // AreaAttribute
            if (string.IsNullOrEmpty(moduleName) == false)
            {
                // Area
                _areaAttribute = new AreaAttribute(moduleName);                
            }
            else
            {
                // Non-Area
                _areaAttribute = new NonAreaAttribute();
            }

            // RouteAttribute
            string routeTemplate = string.Empty;
            if (string.IsNullOrEmpty(moduleName) == false)
            {
                // Area
                routeTemplate = @"[area]/[controller]/[action]";
            }
            else
            {
                // Non-Area
                routeTemplate = @"[controller]/[action]";
            }
            _routeAttribute = new RouteAttribute(routeTemplate);
            _routeAttribute.Name = routeTemplate;
        }


        // AreaAttribute
        string IRouteValueProvider.RouteKey => _areaAttribute.RouteKey;

        string IRouteValueProvider.RouteValue => _areaAttribute.RouteValue;

        // RouteAttribute
        string IRouteTemplateProvider.Template => _routeAttribute.Template;

        int? IRouteTemplateProvider.Order => _routeAttribute.Order;

        string IRouteTemplateProvider.Name => _routeAttribute.Name ?? String.Empty;


        // Class
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        private class NonAreaAttribute : RouteValueAttribute
        {
            public NonAreaAttribute() : base("non-area", "non-area") { }
        }
    }
}
