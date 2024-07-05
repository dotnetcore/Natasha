using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class MainAnalyser
    {
        public static string _asyncCommentTag = "//HE:Async".ToLower();
        internal static string _proxyCommentOPLDebug = "//HE:Debug".ToLower();
        internal static string _proxyCommentOPLRelease = "//HE:Release".ToLower();
        public static void Handle(MethodDeclarationSyntax methodNode, out bool isRelease, out bool isAsync)
        {
            isRelease = false;
            isAsync = false;
            var body = methodNode.Body;
            if (body != null)
            {
                var comment = body.DescendantNodesAndSelf()
                        .SelectMany(node => node.GetLeadingTrivia())
                        .FirstOrDefault(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia));

                if (comment != default)
                {
                    var commentText = comment.ToString().Trim().ToLower();
                    if (commentText.StartsWith(_proxyCommentOPLRelease))
                    {
                        isRelease = true;
                    }
                    else if (commentText.StartsWith(_asyncCommentTag))
                    {
                        isAsync = true;
                    }
                }
            }
        }
    }
}
