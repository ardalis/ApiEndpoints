using System;
using System.Collections.Generic;
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
        private static readonly LocalizableString Description = "MVC will interpret additional public methods on an Endpoint as Actions.  Limit Endpoints to a single Action.";
        private const string Category = "Naming";

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
                var methodSymbol = context.Symbol as IMethodSymbol;

                // not a method declaration
                if (null == methodSymbol) return;

                // ignore everything except 'ordinary' methods (delegates, operators, etc)
                if (methodSymbol.MethodKind != MethodKind.Ordinary) return;

                // ignore statics
                if (methodSymbol.IsStatic) return;

                // isn't public
                if (methodSymbol.DeclaredAccessibility != Accessibility.Public) return;

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

                // gather all public methods
                var allPublicMethods =
                    methodSymbol
                        .ContainingType
                        .GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => 
                            !m.IsStatic &&
                            m.MethodKind == MethodKind.Ordinary &&
                            m.DeclaredAccessibility == Accessibility.Public)
                        .ToList();

                // and sort the methods so the "most correct" is first
                // the 'most correct' will not receive an error message, all others will
                allPublicMethods.Sort(new MostCorrectMethodSorter());

                // if our methodSymbol is the first method, then don't display error
                if (allPublicMethods.First().IsEquivalent(methodSymbol)) return;

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

                if (Debugger.IsAttached) throw;
            }
        }

        public class MostCorrectMethodSorter : IComparer<IMethodSymbol>
        {
            public int Compare(IMethodSymbol x, IMethodSymbol y)
            {
                if ((x.IsOverride || y.IsOverride) && x.IsOverride != y.IsOverride)
                {
                    return x.IsOverride ? -1 : 1;
                }

                if (x.Name == "Handle" && y.Name != "Handle")
                {
                    return -1;
                }

                if (y.Name == "Handle" && x.Name != "Handle")
                {
                    return 1;
                }

                if (x.Name == "HandleAsync" && y.Name != "HandleAsync")
                {
                    return -1;
                }

                if (y.Name == "HandleAsync" && x.Name != "HandleAsync")
                {
                    return 1;
                }

                // give precedence to which ever came first - so always x
                return -1;
            }
        }
    }

    public static class SymbolExtensions
    {
        public static List<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol namedTypeSymbol)
        {
            var baseTypes = new List<INamedTypeSymbol>();

            for (var visitor = namedTypeSymbol.BaseType; visitor != null; visitor = visitor.BaseType)
            {
                if (!baseTypes.Contains(visitor))
                {
                    baseTypes.Add(visitor);
                }
            }

            return baseTypes;
        }

        public static bool IsEquivalent(this IMethodSymbol methodSymbol, IMethodSymbol other)
        {
            if (methodSymbol.Arity != other.Arity ||
                methodSymbol.Name != other.Name ||

                methodSymbol.TypeParameters.Length != other.TypeParameters.Length ||
                methodSymbol.Parameters.Length != other.Parameters.Length)
            {
                return false;
            }

            for (var i = 0; i < methodSymbol.TypeParameters.Length; i++)
            {
                if (methodSymbol.TypeParameters[i].ToDisplayString() !=
                    other.TypeParameters[i].ToDisplayString())
                {
                    return false;
                }
            }

            for (var i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                if (methodSymbol.Parameters[i].ToDisplayString() !=
                    other.Parameters[i].ToDisplayString())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
