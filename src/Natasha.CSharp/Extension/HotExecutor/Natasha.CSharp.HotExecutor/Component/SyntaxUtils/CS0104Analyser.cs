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
            HashSet<SyntaxTrivia> triviaSets = [];
            foreach (SyntaxNode node in root.DescendantNodesAndSelf())
            {
                foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
                {
                    if (triviaSets.Contains(trivia))
                    {
                        continue;
                    }
                    if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                    {
                        var commentText = trivia.ToString().Trim().ToLower();
                        if (commentText.Length > _proxyCommentCS0104Using.Length)
                        {
                            if (commentText.StartsWith(_proxyCommentCS0104Using))
                            {
                                triviaSets.Add(trivia);
                                //#if DEBUG
                                //                            Debug.WriteLine($"找到剔除点：{commentText}");
                                //                            Debug.WriteLine($"整个节点为：{node.ToFullString()}");
                                //#endif
                                var usingStrings = trivia.ToString().Trim().Substring(_proxyCommentCS0104Using.Length, commentText.Length - _proxyCommentCS0104Using.Length);
                                tempSets.UnionWith(usingStrings.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()));
                            }
                        }
                    }
                }
            }
            return tempSets;
        }
    }
}
