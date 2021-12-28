using Microsoft.CodeAnalysis;

public static class DiagnosticsExtension
{
    public static SyntaxNode GetSyntaxNode(this Diagnostic diagnostic,SyntaxNode syntaxNode)
    {
        return syntaxNode.FindNode(diagnostic.Location.SourceSpan);
    }
}

