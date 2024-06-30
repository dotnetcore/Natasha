using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class MethodTriviaRewriter
    {
        public static CompilationUnitSyntax Handle(CompilationUnitSyntax root, Func<string,string?> replaceFunc)
        {

            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            if (methodDeclarations == null)
            {
                return root;
            }
            ConcurrentDictionary<BlockSyntax, BlockSyntax> replaceBodyCache = [];
            foreach (var methodDeclaration in methodDeclarations)
            {
                
                var bodyNode = methodDeclaration.Body;
                if (bodyNode != null)
                {
                    ConcurrentDictionary<SyntaxNode, BlockSyntax>? blockCache = null;
                    var tempDict = bodyNode
                        .DescendantNodesAndSelf()
                        .OfType<BlockSyntax>()
                        .Where(item => item != null && item.Parent != null)
                        .Where(item => item.Parent != null);
                    if (tempDict.Any())
                    {
                        blockCache = new(tempDict.ToDictionary(item => item.Parent!, item => item));
                    }

                    var newBody = GetNewBlockSyntax(bodyNode, replaceFunc, blockCache);

                    if (newBody != null)
                    {
                        replaceBodyCache[bodyNode] = newBody;
                    }
                }
            }


            foreach (var item in replaceBodyCache)
            {
                root = root.ReplaceNode(item.Key, item.Value);
            }
            return root;
        }

        private static BlockSyntax? GetNewBlockSyntax(
                BlockSyntax methodBodySyntax,
                Func<string, string?> replaceFunc,
                ConcurrentDictionary<SyntaxNode, BlockSyntax>? blockCache
                )
        {
            var removeIndexs = new HashSet<int>();
            // 获取方法体
            Dictionary<int, List<StatementSyntax>> addStatementCache = [];
            if (methodBodySyntax.OpenBraceToken.HasLeadingTrivia)
            {
                var (needDelete, newStatements) = HandleTriviaComment(methodBodySyntax.OpenBraceToken.LeadingTrivia, false, replaceFunc);
                if (newStatements.Count>0)
                {
                    addStatementCache[-1] = newStatements;
                }
            }
            var statementCount = methodBodySyntax.Statements.Count;
            // 遍历方法体中的语句
            for (int i = 0; i < statementCount; i++)
            {

                // 获取当前语句
                var statement = methodBodySyntax.Statements[i];
                var (needDelete, newStatements) = GetNewStatmentSyntax(statement, replaceFunc, blockCache);
                if (needDelete)
                {
                    removeIndexs.Add(i);
                }
                if (newStatements.Count>0)
                {
                    addStatementCache[i] = newStatements;
                }

            }

            if (methodBodySyntax.CloseBraceToken.HasLeadingTrivia)
            {
                var (needDelete, newStatements) = HandleTriviaComment(methodBodySyntax.CloseBraceToken.LeadingTrivia, false, replaceFunc);
                if (newStatements.Count > 0)
                {
                    addStatementCache[-2] = newStatements;
                }
            }

            // 如果找到，创建新的方法体列表并排除该语句
            if (removeIndexs.Count > 0 || addStatementCache.Count > 0)
            {
                var statements = methodBodySyntax.Statements;
                List<StatementSyntax> newStatments = [];
                if (addStatementCache.TryGetValue(-1, out var headList))
                {
                    newStatments.AddRange(headList);
                }
                if (statements.Count != 0)
                {
                    for (int i = 0; i < statements.Count; i++)
                    {
                        if (addStatementCache.ContainsKey(i))
                        {
                            newStatments.AddRange(addStatementCache[i]);
                        }
                        if (!removeIndexs.Contains(i))
                        {
                            newStatments.Add(statements[i]);
                        }
                    }
                }
                if (addStatementCache.TryGetValue(-2, out var tailList))
                {
                    newStatments.AddRange(tailList);
                }
                return SyntaxFactory.Block(newStatments);
            }

            return null;
            
        }

        private static (bool needDelete,List<StatementSyntax> newStatements) GetNewStatmentSyntax(
            StatementSyntax statement,
            Func<string, string?> replaceFunc,
            ConcurrentDictionary<SyntaxNode, BlockSyntax>? blockCache)
        {
            bool shouldDelete = false;
            List<StatementSyntax> statementList = [];
            if (statement.HasLeadingTrivia)
            {
                var trivias = statement.GetLeadingTrivia();
                (shouldDelete, statementList) = HandleTriviaComment(trivias, true, replaceFunc);
            }
            if (!shouldDelete)
            {
                if (blockCache != null && blockCache.TryGetValue(statement, out var subBlock))
                {
                    var newBlock = GetNewBlockSyntax(subBlock, replaceFunc, blockCache);
                    if (newBlock != null)
                    {
                        statementList.Add(statement.ReplaceNode(subBlock, newBlock));
                        shouldDelete = true;
                    }
                }
            }

            if (statement.HasTrailingTrivia)
            {
                var trivias = statement.GetTrailingTrivia();
                var (needDelete, newStatements) = HandleTriviaComment(trivias, true, replaceFunc);
                if (!shouldDelete && needDelete)
                {
                    shouldDelete = true;
                }
                statementList.AddRange(newStatements);
            }

            return (shouldDelete, statementList);
        }

        public static (bool needDelete, List<StatementSyntax> newStatements) HandleTriviaComment(SyntaxTriviaList trivias, bool needDeleted, Func<string, string?> replaceFunc)
        {
            bool shouldDelete = false;
            List<StatementSyntax> syntaxList = [];
            foreach (var trivia in trivias)
            {

                if (trivia != null && trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                {
                    var commentText = trivia.ToString();
                    if (commentText.Length > 2)
                    {
                        var result = replaceFunc(commentText);
                        if (result != null)
                        {
                            if (needDeleted && result == string.Empty)
                            {
                                shouldDelete = true;
                            }
                            else
                            {
                                var statementNode = SyntaxFactory.ParseStatement(result);
                                syntaxList.Add(statementNode);
                            }
                        }
                    }
                }
            }
            return (shouldDelete, syntaxList);
        }
    }
}
