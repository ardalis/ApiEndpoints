using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Ardalis.ApiEndpoints.CodeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EndpointHasExtraPublicMethodAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ApiEndpoints101";

        internal static readonly LocalizableString Title = "Endpoint has more than one public method";
        internal static readonly LocalizableString MessageFormat = "Endpoint {0} has additional public method {1}. Endpoints must have only one public method.";
        private static readonly LocalizableString Description = "MVC will interpret additional public methods on an Endpoint as Actions. Limit Endpoints to a single Action.";
        private const string Category = "Naming";

        private static readonly string[] EndpointMethodNames = new[] { "HandleAsync", "Handle" };

        private static readonly DiagnosticDescriptor Rule = new(
#pragma warning disable RS2008 // Enable analyzer release tracking
                DiagnosticId,
#pragma warning restore RS2008 // Enable analyzer release tracking
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeMethodDeclaration, SymbolKind.Method);
        }

        private void AnalyzeMethodDeclaration(SymbolAnalysisContext context)
        {
            try
            {
                if (context.Symbol is not IMethodSymbol methodSymbol || !IsApiAction(methodSymbol))
                {
                    return;
                }

                var isApiEndpoint = methodSymbol.ContainingType
                    .GetBaseTypesAndThis()
                    .Any(t => t.ToString() == "Ardalis.ApiEndpoints.EndpointBase");

                // not a type inheriting EndpointBase
                if (!isApiEndpoint)
                {
                    return;
                }

                // gather all public methods ordered by "most correct" name and line number
                var allApiActions = methodSymbol.ContainingType
                    .GetMembers()
                    .OfType<IMethodSymbol>()
                    .Where(IsApiAction)
                    .OrderByDescending(m => EndpointMethodNames.Contains(m.Name))
                        .ThenBy(m => m.Locations.First().GetLineSpan().StartLinePosition)
                    .ToList();

                // if our methodSymbol is the first method, then don't display error
                if (allApiActions.First().Equals(methodSymbol, SymbolEqualityComparer.Default)) return;

                // at this point, we have a new public method on a EndpointBase that violates the rule
                var diagnostic = Diagnostic.Create(
                    Rule,
                    context.Symbol.Locations.First(),
                    methodSymbol.ContainingType.Name,
                    methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);

                static bool IsApiAction(IMethodSymbol m)
                {
                    return !m.IsStatic
                        && m.MethodKind == MethodKind.Ordinary
                        && m.DeclaredAccessibility == Accessibility.Public;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);

                if (Debugger.IsAttached) throw;
            }
        }
    }
}
