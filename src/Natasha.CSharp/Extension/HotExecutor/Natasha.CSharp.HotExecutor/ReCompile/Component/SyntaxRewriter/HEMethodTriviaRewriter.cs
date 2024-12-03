using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    internal class HEMethodTriviaRewriter : CSharpSyntaxRewriter
    {
        public readonly HashSet<TriviaSyntaxPluginBase> _methodTriviaSyntaxPlugins;
        public HEMethodTriviaRewriter(HashSet<TriviaSyntaxPluginBase> methodTriviaSyntaxPluginBases)
        {
            _methodTriviaSyntaxPlugins = methodTriviaSyntaxPluginBases;
        }
        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                var commentText = trivia.ToString();
                var commentLowerText = commentText.ToLower();
                if (commentText.Length > 2)
                {
                    foreach (var plugin in _methodTriviaSyntaxPlugins)
                    {
                        if (plugin.IsMatch(commentText,commentLowerText))
                        {
                            var result = plugin.Handle(commentText, commentLowerText);
                            if (result != null)
                            {
                                return SyntaxFactory.Comment(result);
                            }
                        }
                    }
                }
            }
            return base.VisitTrivia(trivia);
        }
    }

}
