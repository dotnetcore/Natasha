using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Natasha.CSharp.Compiler.SemanticAnalaysis
{
    internal static class UsingAnalysistor
    {
        internal static readonly Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation> _usingSemanticDelegate;
        static UsingAnalysistor()
        {

            _usingSemanticDelegate = (builder, compilation) =>
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
                               error.RemoveDefaultUsingAndUsingNode(root, errorNodes);
                           }
                           else if (error.Id == "CS8019")
                           {
                               errorNodes.Add(error.GetUsingSyntaxNode(root));
                           }
                           else if (error.Id == "CS0246")
                           {
                               var node = error.GetSyntaxNode(root);
                               if (node.Parent!=null)
                               {
                                   UsingDirectiveSyntax? usingNode = node.Parent as UsingDirectiveSyntax;
                                   if (usingNode != null)
                                   {
                                       DiagnosticsExtension.RemoveUsingInfo(usingNode, errorNodes);
                                   }
                               }
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
