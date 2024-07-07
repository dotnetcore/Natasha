using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natasha.CSharp.HotExecutor.Component
{
    internal static class OnceHandler
    {
        private static string _onceComment = "//Once".ToLower();
        public static void SetOnceComment(string onceComment)
        {
            _onceComment = onceComment.ToLower();
        }
        public static CompilationUnitSyntax Handle(CompilationUnitSyntax root)
        {

            var statementSyntaxes = root
                .DescendantNodesAndSelf()
                .OfType<StatementSyntax>()
                .Where(statement=>statement.HasLeadingTrivia || statement.HasTrailingTrivia);
 
            var removeNodes = new HashSet<SyntaxNode>();
            if (statementSyntaxes!=null)
            {
                foreach (var statement in statementSyntaxes)
                {
                    if (statement.HasLeadingTrivia)
                    {
                        if (ShouldRemoveStatmentSyntax(statement.GetLeadingTrivia()))
                        {
                            removeNodes.Add(statement);
                            continue;
                        }
                    }
                    if (statement.HasTrailingTrivia)
                    {
                        if (ShouldRemoveStatmentSyntax(statement.GetTrailingTrivia()))
                        {
                            removeNodes.Add(statement);
                            continue;
                        }
                    }
                }
                if (removeNodes.Count > 0)
                {
                    root = root.RemoveNodes(removeNodes, SyntaxRemoveOptions.KeepExteriorTrivia)!;
                }
            }
            
            return root;
        }

        private static bool ShouldRemoveStatmentSyntax(SyntaxTriviaList leadingTrivias)
        {
            var trivias = leadingTrivias.Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia));

            if (trivias != null)
            {
                foreach (var trivia in trivias)
                {
                    var commentText = trivia.ToString();
                    if (commentText.Length > 2)
                    {
                        var commentLowerText = commentText.ToLower();
                        if (commentLowerText.StartsWith(_onceComment))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
