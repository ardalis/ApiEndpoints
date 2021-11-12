using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ardalis.ApiEndpoints.CodeAnalyzers
{
  [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndpointHasExtraPublicMethodCodeFixProvider)), Shared]
  public class EndpointHasExtraPublicMethodCodeFixProvider : CodeFixProvider
  {
    private const string MakeInternalTitle = "Make additional method internal.";
    private const string MakePrivateTitle = "Make additional method private.";

    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(EndpointHasExtraPublicMethodAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
      // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
      return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
      var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
      if (root == null)
      {
        return;
      }

      var diagnostic = context.Diagnostics.First();
      var diagnosticSpan = diagnostic.Location.SourceSpan;

      // Find the method declaration identified by the diagnostic.
      var declaration = root
          .FindToken(diagnosticSpan.Start)
          .Parent!
          .AncestorsAndSelf()
          .OfType<MethodDeclarationSyntax>()
          .First();

      // Register a code action that will invoke the fix.
      context.RegisterCodeFix(
          CodeAction.Create(
              title: MakeInternalTitle,
              createChangedDocument: c => ChangeMethodKindAsync(context.Document, declaration, SyntaxKind.InternalKeyword, c),
              equivalenceKey: MakeInternalTitle),
          diagnostic);

      context.RegisterCodeFix(
          CodeAction.Create(
              title: MakePrivateTitle,
              createChangedDocument: c => ChangeMethodKindAsync(context.Document, declaration, SyntaxKind.PrivateKeyword, c),
              equivalenceKey: MakePrivateTitle),
          diagnostic);
    }

    private async Task<Document> ChangeMethodKindAsync(
        Document document,
        MethodDeclarationSyntax method,
        SyntaxKind targetKind,
        CancellationToken cancellationToken)
    {
      try
      {
        var modifierList =
            // create the new modifier list, but replace the public token
            // goal is to preserve order
            method
                .Modifiers
                .Select(x =>
                    x.Kind() is SyntaxKind.PublicKeyword
                        ? SyntaxFactory.Token(targetKind)
                        : x)
                .ToList();

        // remove the public modifier
        var newMethod = method.WithModifiers(new SyntaxTokenList(modifierList));

        var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var root = await tree!.GetRootAsync(cancellationToken).ConfigureAwait(false);

        var newRoot = root.ReplaceNode(method, newMethod);

        return document.WithSyntaxRoot(newRoot);
      }
      catch (Exception e)
      {
        Debug.Write(e);

        if (Debugger.IsAttached)
          throw;

        return document;
      }
    }
  }
}
