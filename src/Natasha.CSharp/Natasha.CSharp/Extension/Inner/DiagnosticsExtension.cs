using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.CSharp.Extension.Inner
{
    internal static class DiagnosticsExtension
    {
        internal static SyntaxNode GetSyntaxNode(this Diagnostic diagnostic, CompilationUnitSyntax root)
        {
            return root.FindNode(diagnostic.Location.SourceSpan);
        }
        internal static UsingDirectiveSyntax GetUsingSyntaxNode(this Diagnostic diagnostic, CompilationUnitSyntax root)
        {
            var node = GetSyntaxNode(diagnostic, root);
            while (node is not UsingDirectiveSyntax)
            {
                node = node!.Parent;
            }
            return (UsingDirectiveSyntax)node;
        }
        internal static void RemoveDefaultUsingAndUsingNode(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
        {
            var usingNode = GetUsingSyntaxNode(diagnostic, root);
            RemoveUsingInfo(usingNode, removeCollection);
        }

        internal static void RemoveUsingInfo(this UsingDirectiveSyntax usingDirectiveSyntax, HashSet<SyntaxNode> removeCollection)
        {
            removeCollection.Add(usingDirectiveSyntax);
            DefaultUsing.Remove(usingDirectiveSyntax.Name.ToString());
        }

        internal static void RemoveUsingDiagnostics(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
        {
            var usingNode = GetUsingSyntaxNode(diagnostic, root);
            var usingNodes = (from usingDeclaration in root.Usings
                              where usingDeclaration.Name.ToString().StartsWith(usingNode.Name.ToString())
                              select usingDeclaration).ToList();

            removeCollection.UnionWith(usingNodes);
            DefaultUsing.Remove(usingNodes.Select(item => item.Name.ToString()));
        }
    }
}


