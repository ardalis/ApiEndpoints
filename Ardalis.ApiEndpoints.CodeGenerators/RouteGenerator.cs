using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Ardalis.ApiEndpoints.CodeGenerators
{
    [Generator]
    public class RouteGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) 
        {
            context.RegisterForSyntaxNotifications(() => new ChildClassReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ChildClassReceiver baseEndpointReceiver)
            {
                return;
            }

            List<ITypeSymbol> baseEndpointChildren = baseEndpointReceiver.Candidates
                .Select(c => context.Compilation.GetSemanticModel(c.SyntaxTree).GetDeclaredSymbol(c))
                .OfType<ITypeSymbol>()
                .Where(s => s.GetBaseTypesAndThis().Any(t => t.ToString() == "Ardalis.ApiEndpoints.EndpointBase"))
                .ToList();

            foreach (ITypeSymbol targetType in baseEndpointChildren)
            {
                string sourceCode = GenerateRoute(targetType);
                context.AddSource($"{targetType.Name}.Route.cs", sourceCode);
            }
        }

        private string GenerateRoute(ITypeSymbol targetType) =>
$@"using Microsoft.AspNetCore.Mvc;

namespace {targetType.ContainingNamespace}
{{
    [Route(""api/{targetType.ContainingNamespace.Name}"")]
    partial class {targetType.Name} {{ }}
}}";
    }
}
