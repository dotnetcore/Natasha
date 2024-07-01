using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class ConsoleWriter
    {
        public static BlockSyntax? Handle(BlockSyntax blockSyntax)
        {
            if (blockSyntax.Statements.Count > 0)
            {
                var node = blockSyntax.Statements.Last();
                if (node.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ExpressionStatement))
                {
                    if (node.ToString().StartsWith("Console.Read"))
                    {
                        return blockSyntax.RemoveNode(node, SyntaxRemoveOptions.KeepExteriorTrivia);
                    }
                }
            }
            return null;
        }
    }
}
