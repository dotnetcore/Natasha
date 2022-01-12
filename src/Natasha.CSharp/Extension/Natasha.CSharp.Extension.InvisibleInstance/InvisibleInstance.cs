using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;


internal static class InvisibleInstance
{
    internal static Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation> Creator(string argument)
    {

        return (buidler,compilation) =>
        {

            IdentifierNameSyntax instance = SyntaxFactory.IdentifierName(argument);
            SyntaxTree tree = compilation.SyntaxTrees[0];
            var semantiModel = compilation.GetSemanticModel(tree);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var methodNodes = from methodDeclaration in root.DescendantNodes()
                            .OfType<MethodDeclarationSyntax>()
                              select methodDeclaration;

            var diagnostics = new HashSet<Location>(semantiModel.GetDiagnostics().Where(item => item.Id == "CS0103").Select(item => item.Location));
            foreach (var methodNode in methodNodes)
            {

                var body = methodNode.Body;
                if (body!=null)
                {
                    var nodes = from CS0103Node in body.DescendantNodes().OfType<IdentifierNameSyntax>()
                                where diagnostics.Contains(CS0103Node.GetLocation())
                                select CS0103Node;

                    foreach (var node in nodes)
                    {

                        var symbolInfo = semantiModel.GetSymbolInfo(node);
                        if (symbolInfo.Symbol == null && symbolInfo.CandidateSymbols.Length == 0)
                        {

                            var member = SyntaxFactory.IdentifierName(node.Identifier.ValueText.Trim());
                            var newNode = SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                instance,
                                member);
                            root = root.ReplaceNode(node, newNode);

                        }
                    }
                }
                
            }

            return compilation.ReplaceSyntaxTree(tree, root.SyntaxTree);

        };
    }

    internal static Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation> Creator()
    {

        return (builder, compilation) =>
        {


            SyntaxTree tree = compilation.SyntaxTrees[0];
            var semantiModel = compilation.GetSemanticModel(tree);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var methodNodes = from methodDeclaration in root.DescendantNodes()
                            .OfType<MethodDeclarationSyntax>()
                              select methodDeclaration;


            var diagnostics = new HashSet<Location>(semantiModel.GetDiagnostics().Where(item=>item.Id == "CS0103").Select(item=>item.Location));
            foreach (var methodNode in methodNodes)
            {

                string argument = string.Empty;
                if (methodNode.ParameterList != null)
                {
                    if (methodNode.ParameterList.Parameters.Count > 0)
                    {
                        argument = methodNode.ParameterList.Parameters[0].Identifier.ValueText;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                IdentifierNameSyntax instance = SyntaxFactory.IdentifierName(argument);
                var body = methodNode.Body;
                if (body!=null)
                {
                    var nodes = from CS0103Node in body.DescendantNodes().OfType<IdentifierNameSyntax>()
                                where diagnostics.Contains(CS0103Node.GetLocation())
                                select CS0103Node;

                    foreach (var node in nodes)
                    {

                        var symbolInfo = semantiModel.GetSymbolInfo(node);
                        if (symbolInfo.Symbol == null && symbolInfo.CandidateSymbols.Length == 0)
                        {

                            var member = SyntaxFactory.IdentifierName(node.Identifier.ValueText.Trim());
                            var newNode = SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                instance,
                                member);
                            root = root.ReplaceNode(node, newNode);

                        }
                    }
                }

            }
            return compilation.ReplaceSyntaxTree(tree, root.SyntaxTree);

        };
    }
}

