using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

public static class DiagnosticsExtension
{
    public static SyntaxNode GetSyntaxNode(this Diagnostic diagnostic, CompilationUnitSyntax root)
    {
        return root.FindNode(diagnostic.Location.SourceSpan);
    }
    public static UsingDirectiveSyntax GetUsingSyntaxNode(this Diagnostic diagnostic, CompilationUnitSyntax root)
    {
        var node = GetSyntaxNode(diagnostic, root);
        while (node is not UsingDirectiveSyntax)
        {
            node = node!.Parent;
        }
        return (UsingDirectiveSyntax)node;
    }
    public static void RemoveUsingDiagnostic(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
    {
        var usingNode = GetUsingSyntaxNode(diagnostic,root);
        removeCollection.Add(usingNode);
        DefaultUsing.Remove(usingNode!.Name.ToString());
    }

    public static void RemoveUsingDiagnostics(this Diagnostic diagnostic, CompilationUnitSyntax root, HashSet<SyntaxNode> removeCollection)
    {
        var usingNode = GetUsingSyntaxNode(diagnostic, root);
        var usingNodes = (from usingDeclaration in root.Usings
                          where usingDeclaration.Name.ToString().StartsWith(usingNode.Name.ToString())
                          select usingDeclaration).ToList();

        removeCollection.UnionWith(usingNodes);
        DefaultUsing.Remove(usingNodes.Select(item => item.Name.ToString()));
    }
}

