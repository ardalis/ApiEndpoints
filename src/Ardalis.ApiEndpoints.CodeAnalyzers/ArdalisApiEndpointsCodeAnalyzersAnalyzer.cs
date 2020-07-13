using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Ardalis.ApiEndpoints.CodeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ArdalisApiEndpointsCodeAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ArdalisApiEndpointsCodeAnalyzers";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

            // does not have const
            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                return;
            }

            var variableTypeName = localDeclaration.Declaration.Type;
            var variableType = context.SemanticModel.GetTypeInfo(variableTypeName).ConvertedType;

            // Ensure that all variables in the local declaration have initializers that
            // are assigned with constant values.
            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                var initializer = variable.Initializer;
                if (initializer == null)
                {
                    return;
                }

                var constantValue = context.SemanticModel.GetConstantValue(initializer.Value);
                if (!constantValue.HasValue)
                {
                    return;
                }

                // Ensure that the initializer value can be converted to the type of the
                // local declaration without a user-defined conversion.
                var conversion = context.SemanticModel.ClassifyConversion(initializer.Value, variableType);
                if (!conversion.Exists || conversion.IsUserDefined)
                {
                    return;
                }

                // Special cases:
                //  * If the constant value is a string, the type of the local declaration
                //    must be System.String.
                //  * If the constant value is null, the type of the local declaration must
                //    be a reference type.
                if (constantValue.Value is string)
                {
                    if (variableType.SpecialType != SpecialType.System_String)
                    {
                        return;
                    }
                }
                else if (variableType.IsReferenceType && constantValue.Value != null)
                {
                    return;
                }
            }

            // Perform data flow analysis on the local declaration.
            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

            foreach (var variable in localDeclaration.Declaration.Variables)
            {
                // Retrieve the local symbol for each variable in the local declaration
                // and ensure that it is not written outside of the data flow analysis region.
                var variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable);
                if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
                {
                    return;
                }
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
        }
    }
}
