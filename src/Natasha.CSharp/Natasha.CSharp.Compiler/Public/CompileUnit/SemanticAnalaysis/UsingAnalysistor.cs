using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Natasha.CSharp.Compiler.SemanticAnalaysis
{
    internal static class UsingAnalysistor
    {
        internal static readonly Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> _usingSemanticDelegate;
        static UsingAnalysistor()
        {

            _usingSemanticDelegate = (builder, compilation, ignoreAccessibility) =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
#if DEBUG
                    Stopwatch stopwatch = new();
                    stopwatch.Start();
#endif
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

                    SemanticModel semantiModel = compilation.GetSemanticModel(tree, ignoreAccessibility);
#if DEBUG
                    stopwatch.RestartAndShowCategoreInfo("[Semantic]", "语义节点获取", 3);
#endif

                    var errors = semantiModel.GetDiagnostics();
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义诊断获取", 3);
                    stopwatch.Restart();
#endif

                    if (errors.Length > 0)
                    {
                        var errorNodes = new HashSet<SyntaxNode>();
                        for (int i = 0; i < errors.Length; i++)
                        {
                            var error = errors[i];
                            if (error.Id == "CS0434")
                            {
                                error.RemoveDefaultUsingAndUsingNode(root, errorNodes);
                            }
                            else if (error.Id == "CS8019")
                            {
                                var node = error.GetTypeSyntaxNode<UsingDirectiveSyntax>(root);
                                if (node != null)
                                {
                                    errorNodes.Add(node);
                                }

                            }
                            else if (error.Id == "CS0246")
                            {
                                var node = error.GetTypeSyntaxNode<UsingDirectiveSyntax>(root);
                                if (node != null)
                                {
                                    NatashaDiagnosticsExtension.RemoveUsingAndNode(node, errorNodes);
                                }
                            }
                            else if (error.Id == "CS0234")
                            {
                                error.RemoveUsingAndNodesFromStartName(root, errorNodes);
                            }
                        }

#if DEBUG
                        stopwatch.RestartAndShowCategoreInfo("[Semantic]", "语义节点筛查", 3);

#endif
                        if (errorNodes.Count > 0)
                        {
                            compilation = compilation.ReplaceSyntaxTree(tree, root.RemoveNodes(errorNodes, SyntaxRemoveOptions.KeepNoTrivia)!.SyntaxTree);
                        }

#if DEBUG
                        stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义节点替换", 3);
#endif
                    }

                }
                return compilation;
            };
        }

    }
}
