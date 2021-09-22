using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Updates Swagger document to support ApiEndpoints.<br/><br/>
        /// For controllers inherited from <see cref="EndpointBase"/>:<br/>
        /// - Replaces action Tag with <c>[namespace]</c><br/>
        /// </summary>
        public static void UseApiEndpoints(this SwaggerGenOptions options)
        {
            options.TagActionsBy(EndpointNamespaceOrDefault);
        }

        private static IList<string?> EndpointNamespaceOrDefault(ApiDescription api)
        {
            if (api.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            {
                throw new InvalidOperationException($"Unable to determine tag for endpoint: {api.ActionDescriptor.DisplayName}");
            }

            if (actionDescriptor.ControllerTypeInfo.GetBaseTypesAndThis().Any(t => t == typeof(EndpointBase)))
            {
                return new[] { actionDescriptor.ControllerTypeInfo.Namespace?.Split('.').Last() };
            }

            return new[] { actionDescriptor.ControllerName };
        }
    }
}
