using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Ardalis.ApiEndpoints.CodeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EndpointHasExtraPublicMethodAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ApiEndpoints101";

        internal static readonly LocalizableString Title = "Endpoint has more than one public method";
        internal static readonly LocalizableString MessageFormat = "Endpoint {0} has additional public method {1}. Endpoints must have only one public method.";
        private static readonly LocalizableString Description = "MVC will interpret additional public methods on an Endpoint as Actions.  Limit Endpoints to a single Action";
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = 
            new DiagnosticDescriptor(
                DiagnosticId, 
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
            //string name = "";  
            try
            {
                var methodSymbol = context.Symbol as IMethodSymbol;

                if (null == methodSymbol)
                    return;

                //name += methodSymbol.MethodKind.ToString();

                if (methodSymbol.MethodKind == MethodKind.Constructor) return;
               
                var isApiEndpoint =
                    methodSymbol
                        .ContainingType
                        .GetAllBaseTypes()
                        .Any(x => 
                            x.Name == "BaseEndpoint" || 
                            x.Name == "BaseAsyncEndpoint");

                // not a type inheriting BaseEndpoint
                if (!isApiEndpoint)
                    return;

                // isn't a new public method
                if (methodSymbol.IsOverride || methodSymbol.DeclaredAccessibility != Accessibility.Public) 
                    return;

                //name += ";" + methodSymbol.Name;

                // at this point, we have a new public method on a BaseEndpoint that violates the rule
                var diagnostic = Diagnostic.Create(
                    Rule, 
                    context.Symbol.Locations.FirstOrDefault(),
                    methodSymbol.ContainingType.Name,
                    methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
            catch (Exception e)
            {
                Debug.Write(e);

                if (Debugger.IsAttached)
                    throw;
            }
        }
    }

    public static class NamedTypeSymbolExtensions
    {
        public static List<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol namedTypeSymbol)
        {
            var baseTypes = new List<INamedTypeSymbol>();

            for (var visitor = namedTypeSymbol.BaseType; visitor != null; visitor = visitor.BaseType)
            {
                if (!baseTypes.Contains(visitor))
                    baseTypes.Add(visitor);
            }

            return baseTypes;
        }
    }
}
