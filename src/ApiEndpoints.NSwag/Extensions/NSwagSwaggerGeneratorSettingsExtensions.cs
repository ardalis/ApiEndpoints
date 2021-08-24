using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.ApiEndpoints;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NSwagSwaggerGeneratorSettingsExtensions
    {
        public static void TagEndpointsByNamespace(this AspNetCoreOpenApiDocumentGeneratorSettings settings)
        {
            settings.OperationProcessors.Add(new EndpointOperationProcessor());
        }

        private class EndpointOperationProcessor : IOperationProcessor
        {
            public bool Process(OperationProcessorContext context)
            {
                if (context.ControllerType.GetBaseTypesAndThis().Any(t => t == typeof(EndpointBase)))
                {
                    context.OperationDescription.Operation.Tags = new List<string> { context.ControllerType.Namespace.Split('.').Last() };
                }

                return true;
            }
        }
    }
}
