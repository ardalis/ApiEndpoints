using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.ApiEndpoints;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace ApiEndpoints.NSwag
{
    internal class EndpointOperationProcessor : IOperationProcessor
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
