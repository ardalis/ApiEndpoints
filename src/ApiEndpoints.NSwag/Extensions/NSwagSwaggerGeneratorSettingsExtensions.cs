using ApiEndpoints.NSwag;
using NSwag.Generation.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NSwagSwaggerGeneratorSettingsExtensions
    {
        public static void TagEndpointsByNamespace(this AspNetCoreOpenApiDocumentGeneratorSettings settings)
        {
            settings.OperationProcessors.Add(new EndpointOperationProcessor());
        }
    }
}
