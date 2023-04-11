using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MDP.AspNetCore
{
    public static class SwaggerMiddlewareExtensions
    {
        // Methods 
        internal static IApplicationBuilder UseSwaggerDefaults(this IApplicationBuilder app)
        {
            #region Contracts

            if (app == null) throw new ArgumentException($"{nameof(app)}=null");
         
            #endregion

            // ApiDescriptionGroupList
            var apiDescriptionGroupList = app.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>()?.ApiDescriptionGroups?.Items;
            if (apiDescriptionGroupList == null) throw new InvalidOperationException($"{apiDescriptionGroupList}=null");

            // ApiDescriptionGroup
            foreach (var apiDescriptionGroup in apiDescriptionGroupList)
            {
                // ApiDescription
                foreach (var apiDescription in apiDescriptionGroup.Items)
                {
                    // Require
                    if (string.IsNullOrEmpty(apiDescription.HttpMethod) == false) continue;

                    // HttpMethod
                    apiDescription.HttpMethod = "POST";
                }
            }

            // Return
            return app;
        }
    }
}
