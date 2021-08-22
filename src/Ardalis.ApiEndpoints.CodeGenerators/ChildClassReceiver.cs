using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ardalis.ApiEndpoints.CodeGenerators
{
    public sealed class ChildClassReceiver : ISyntaxReceiver
    {
        private readonly List<ClassDeclarationSyntax> _candidates = new();

        public IReadOnlyList<ClassDeclarationSyntax> Candidates => _candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.BaseList?.Types.Any() == true)
            {
                _candidates.Add(classSyntax);
            }
        }
    }
}
