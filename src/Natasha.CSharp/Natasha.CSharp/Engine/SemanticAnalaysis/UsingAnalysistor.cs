using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.CSharp.Engine.SemanticAnalaysis
{
    public static class UsingAnalysistor
    {

        public static Func<CSharpCompilation, CSharpCompilation> Creator()
        {

            return (compilation) =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
#if DEBUG
                    Stopwatch stopwatch = new();
                    stopwatch.Start();
#endif
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语法根节点获取", 3);
                    stopwatch.Restart();
#endif
                    var semantiModel = compilation.GetSemanticModel(tree);
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义节点获取", 3);
                    stopwatch.Restart();
#endif
                    var errors = semantiModel!.GetDiagnostics();

#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义诊断获取", 3);
                    stopwatch.Restart();
#endif


                    if (errors.Length > 0)
                    {
                        var editor = new SyntaxEditor(root, new AdhocWorkspace());
#if DEBUG
                        stopwatch.StopAndShowCategoreInfo("[Semantic]", "创建Adhoc", 3);
                        stopwatch.Restart();
#endif
                        var errorNodes = new HashSet<SyntaxNode>();
                        for (int i = 0; i < errors.Length; i++)
                        {
                            var error = errors[i];
                            if (error.Id == "CS0434")
                            {
                                error.RemoveUsingDiagnostic(root, errorNodes);
                            }
                            else if (error.Id == "CS8019")
                            {
                                errorNodes.Add(error.GetUsingSyntaxNode(root));
                            }
                            else if (error.Id == "CS0246")
                            {
                                error.RemoveUsingDiagnostic(root, errorNodes);
                            }
                            else if (error.Id == "CS0234")
                            {
                                error.RemoveUsingDiagnostics(root, errorNodes);
                            }
                        }

                        foreach (var item in errorNodes)
                        {
                            editor.RemoveNode(item);
                        }
                        compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
#if DEBUG
                        stopwatch.StopAndShowCategoreInfo("[Semantic]", "语法过滤", 3);
#endif
                    }

                }
                return compilation;
            };
        }

    }
}
