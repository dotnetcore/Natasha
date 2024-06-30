using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal class CS0104Analyser
    {
        public static string _proxyCommentCS0104Using = "//HE:CS0104".ToLower();
        public static HashSet<string> Handle(CompilationUnitSyntax root)
        {
            HashSet<string> tempSets = [];
            var comments = root.DescendantNodesAndSelf()
                .SelectMany(node => node.GetLeadingTrivia())
                .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                .Distinct();

            foreach (var comment in comments)
            {
                var commentText = comment.ToString().Trim().ToLower();
                if (commentText.StartsWith(_proxyCommentCS0104Using))
                {
                    var usingStrings = commentText[_proxyCommentCS0104Using.Length..].Split(';', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var usingString in usingStrings)
                    {
                        tempSets.Add(usingString.Trim());
                    }
                }
            }
            return tempSets;
        }
    }
}
