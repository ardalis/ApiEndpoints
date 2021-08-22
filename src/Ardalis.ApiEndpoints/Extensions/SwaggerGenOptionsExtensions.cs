using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Ardalis.ApiEndpoints.Extensions
{
    public class SwaggerGenOptionsExtensions
    {
        public static IList<string> EndpointNamespace(ApiDescription api)
        {
            if (api.ActionDescriptor is ControllerActionDescriptor actionDescriptor
                && actionDescriptor.ControllerTypeInfo.GetBaseTypesAndThis().Any(t => t == typeof(EndpointBase)))
            {
                return new[] { actionDescriptor.ControllerTypeInfo.Namespace.Split('.').Last() };
            }

            return null;
        }
    }
}
