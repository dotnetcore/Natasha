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
        public static CompilationUnitSyntax Handle(CompilationUnitSyntax root, Func<string, string?> replaceFunc)
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
                        blockCache = new(tempDict.ToDictionary(item =>
                        {

                            SyntaxNode parent = item.Parent!;
                            while (
                            !parent.IsKind(SyntaxKind.MethodDeclaration) && 
                            parent is not StatementSyntax && 
                            parent is not CatchClauseSyntax && 
                            parent is not FinallyClauseSyntax &&
                            parent is not ElseClauseSyntax)
                            {
                                parent = parent.Parent!;
                            }
                            return parent;

                        }, item => item));
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
                BlockSyntax bodySyntax,
                Func<string, string?> replaceFunc,
                ConcurrentDictionary<SyntaxNode, BlockSyntax>? blockCache
                )
        {
            var removeIndexs = new HashSet<int>();
            // 获取方法体
            Dictionary<int, List<StatementSyntax>> addStatementCache = [];
            if (bodySyntax.OpenBraceToken.HasLeadingTrivia)
            {
                var (needDelete, newStatements) = HandleTriviaComment(bodySyntax.OpenBraceToken.LeadingTrivia, false, replaceFunc);
                if (newStatements.Count > 0)
                {
                    addStatementCache[-1] = newStatements;
                }
            }
            var statementCount = bodySyntax.Statements.Count;
            // 遍历方法体中的语句
            for (int i = 0; i < statementCount; i++)
            {

                // 获取当前语句
                var statement = bodySyntax.Statements[i];

                var (needDelete, newStatements) = GetNewStatmentSyntax(statement, replaceFunc, blockCache);
                if (needDelete)
                {
                    removeIndexs.Add(i);
                }
                if (newStatements.Count > 0)
                {
                    addStatementCache[i] = newStatements;
                }
            }

            if (bodySyntax.CloseBraceToken.HasLeadingTrivia)
            {
                var (needDelete, newStatements) = HandleTriviaComment(bodySyntax.CloseBraceToken.LeadingTrivia, false, replaceFunc);
                if (newStatements.Count > 0)
                {
                    addStatementCache[-2] = newStatements;
                }
            }

            // 如果找到，创建新的方法体列表并排除该语句
            if (removeIndexs.Count > 0 || addStatementCache.Count > 0)
            {
                var statements = bodySyntax.Statements;
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

        private static (bool needDelete, List<StatementSyntax> newStatements) GetNewStatmentSyntax(
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
                if (blockCache != null)
                {
                    if (statement is TryStatementSyntax trySyntax)
                    {
                        BlockSyntax newTryBlock = trySyntax.Block;
                        if (blockCache.TryGetValue(trySyntax, out var subBlock))
                        {
                            var newTempTryBlock = GetNewBlockSyntax(trySyntax.Block, replaceFunc, blockCache);
                            if (newTempTryBlock != null)
                            {
                                newTryBlock = newTempTryBlock;
                            }
                        }
                        SyntaxList<CatchClauseSyntax> newCatchList = [];
                        foreach (var catchStatement in trySyntax.Catches)
                        {
                            if (blockCache.TryGetValue(catchStatement, out var subCacheBlock))
                            {
                                var newBlock = GetNewBlockSyntax(subCacheBlock, replaceFunc, blockCache);
                                if (newBlock != null)
                                {
                                    newCatchList = newCatchList.Add(catchStatement.ReplaceNode(subCacheBlock, newBlock));
                                }
                                else
                                {
                                    newCatchList = newCatchList.Add(catchStatement);
                                }
                            }
                        }
                        FinallyClauseSyntax? newFinally = trySyntax.Finally;
                        if (trySyntax.Finally != null && blockCache.TryGetValue(trySyntax.Finally, out var subFinallyBlock))
                        {
                            var newBlock = GetNewBlockSyntax(subFinallyBlock, replaceFunc, blockCache);
                            if (newBlock != null)
                            {
                                newFinally = SyntaxFactory.FinallyClause(newBlock);
                            }
                        }
                        trySyntax = SyntaxFactory.TryStatement(newTryBlock, newCatchList, newFinally);
                        statementList.Add(trySyntax);
                        shouldDelete = true;
                    }
                    else if (statement is IfStatementSyntax ifSyntax && blockCache.TryGetValue(statement, out var subIfBlock))
                    {
                        StatementSyntax newIfStatmentBlock = subIfBlock;
                        ElseClauseSyntax? newElseClause = ifSyntax.Else;
                        var newIfBlock = GetNewBlockSyntax(subIfBlock, replaceFunc, blockCache);
                        if (newIfBlock != null)
                        {
                            newIfStatmentBlock = newIfBlock;
                        }
                        if (ifSyntax.Else != null)
                        {
                            if (ifSyntax.Else.Statement is IfStatementSyntax elseIfSyntax)
                            {
                                var tempElseResult = GetNewStatmentSyntax(
                                    elseIfSyntax
                                    , replaceFunc, blockCache);
                                if (tempElseResult.needDelete)
                                {
                                    var newTempIfStatementSyntax = (tempElseResult.newStatements[0] as IfStatementSyntax)!;
                                    newElseClause = SyntaxFactory.ElseClause(newTempIfStatementSyntax);
                                }
                                
                            }
                            else if (ifSyntax.Else.Statement is BlockSyntax elseBlockSyntax && blockCache.TryGetValue(ifSyntax.Else, out var subElseIfBlock))
                            {
                                var newElseBlockSyntax = GetNewBlockSyntax(
                                    subElseIfBlock
                                    , replaceFunc, blockCache);
                                if (newElseBlockSyntax != null) {

                                    newElseClause = SyntaxFactory.ElseClause(newElseBlockSyntax);
                                }
                                
                            }
                        }

                        statementList.Add(SyntaxFactory.IfStatement(ifSyntax.Condition, newIfStatmentBlock, newElseClause));
                        shouldDelete = true;
                    }
                    else
                    {
                        if (blockCache.TryGetValue(statement, out var subBlock))
                        {
                            var newBlock = GetNewBlockSyntax(subBlock, replaceFunc, blockCache);
                            if (newBlock != null)
                            {
                                statementList.Add(statement.ReplaceNode(subBlock, newBlock));
                                shouldDelete = true;
                            }
                        }
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
