using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class OptimizationAnalyser
    {
        internal static string _proxyCommentOPLDebug = "//HE:Debug".ToLower();
        internal static string _proxyCommentOPLRelease = "//HE:Release".ToLower();
        public static bool? Handle(MethodDeclarationSyntax methodNode)
        {
            var body = methodNode.Body;
            if (body != null)
            {
                var comment = body.DescendantNodesAndSelf()
                        .SelectMany(node => node.GetLeadingTrivia())
                        .FirstOrDefault(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia));

                if (comment != default)
                {
                    var commentText = comment.ToString().Trim().ToLower();
                    if (commentText.StartsWith(_proxyCommentOPLDebug))
                    {
                        return false;
                    }
                    else if (commentText.StartsWith(_proxyCommentOPLRelease))
                    {
                        return true;
                    }
                }
            }
            return null;
        }
    }
}
