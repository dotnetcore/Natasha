using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


internal static class InvisibleInstance
{
    internal static Func<CSharpCompilation, CSharpCompilation> Creator(string argument)
    {


        return (compilation) =>
        {

            IdentifierNameSyntax instance = SyntaxFactory.IdentifierName(argument);
            SyntaxTree tree = compilation.SyntaxTrees[0];
            var semantiModel = compilation.GetSemanticModel(tree);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var methodNodes = from methodDeclaration in root.DescendantNodes()
                            .OfType<MethodDeclarationSyntax>()
                              select methodDeclaration;

            var diagnostics = new HashSet<Location>(semantiModel.GetDiagnostics().Where(item => item.Id == "CS0103").Select(item => item.Location));
            var eiditor = new SyntaxEditor(root, new AdhocWorkspace());
            foreach (var methodNode in methodNodes)
            {


                var body = methodNode.Body;
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
                        eiditor.ReplaceNode(node, newNode);

                    }
                }
            }

            return compilation.ReplaceSyntaxTree(tree, eiditor.GetChangedRoot().SyntaxTree);

        };
    }

    internal static Func<CSharpCompilation, CSharpCompilation> Creator()
    {

        return (compilation) =>
        {


            SyntaxTree tree = compilation.SyntaxTrees[0];
            var semantiModel = compilation.GetSemanticModel(tree);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var methodNodes = from methodDeclaration in root.DescendantNodes()
                            .OfType<MethodDeclarationSyntax>()
                              select methodDeclaration;


            var diagnostics = new HashSet<Location>(semantiModel.GetDiagnostics().Where(item=>item.Id == "CS0103").Select(item=>item.Location));
            var eiditor = new SyntaxEditor(root, new AdhocWorkspace());
            foreach (var methodNode in methodNodes)
            {

                string argument = default;
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
                        eiditor.ReplaceNode(node, newNode);

                    }
                }
            }
            return compilation.ReplaceSyntaxTree(tree, eiditor.GetChangedRoot().SyntaxTree);

        };
    }
}

