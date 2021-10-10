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
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
#endif
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var text = root.ToString();
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语法根节点获取", 3);
                    stopwatch.Restart();
#endif
                    var semantiModel = compilation.GetSemanticModel(tree);
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义节点获取", 3);
                    stopwatch.Restart();
#endif
                    var errors = semantiModel.GetDiagnostics();
                    //errors = semantiModel.getdis();
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义诊断获取", 3);
                    stopwatch.Restart();
#endif

                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    var cs8019errors = new HashSet<Location>();
                    var cs0246Recoder = new HashSet<string>();
                    var cs0246errors = new HashSet<Diagnostic>();
                    var cs0234errors = new HashSet<Diagnostic>();
                    var cs0246NoUseUsings = new HashSet<string>();

                    foreach (var item in errors)
                    {
                        if (item.Id == "CS8019")
                        {
                            cs8019errors.Add(item.Location);
                        }
                        else if (item.Id == "CS0246")
                        {
                            var message = item.GetMessage();
                            if (!cs0246Recoder.Contains(message))
                            {
                                cs0246Recoder.Add(message);
                                cs0246errors.Add(item);
                            }
                        }
                        else if (item.Id == "CS0234")
                        {
                            cs0234errors.Add(item);
                        }
                    }
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "诊断信息筛取", 3);
                    stopwatch.Restart();
#endif
                    object _lock = new object();
                    List<Task> tasks = new List<Task>();
                    if (cs8019errors.Count > 0)
                    {

                        tasks.Add(Task.Run(() =>
                        {
                            var results = CS8019Analaysistor.Handler(root, cs8019errors);
                            lock (_lock)
                            {
                                removeCache.UnionWith(results);
                            }
                        }));

                    }
                    if (cs0246errors.Count>0)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            cs0246NoUseUsings.UnionWith(CS0246Analaysistor.GetUnableUsing(cs0246errors, text));
                            var results = CS0246Analaysistor.Handler(root, cs0246NoUseUsings);
                            lock (_lock)
                            {
                                removeCache.UnionWith(results);
                            }
                        }));
                    }
                    if (cs0234errors.Count>0)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            foreach (var item in cs0234errors)
                            {
                                lock (_lock)
                                {
                                    removeCache.UnionWith(CS0234Analaysistor.Handler(root, item));
                                }
                            }
                        }));
                    }
                    Task.WaitAll(tasks.ToArray());
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语法过滤", 3);
                    stopwatch.Restart();
#endif
                    if (removeCache.Count > 0)
                    {
                        var editor = new SyntaxEditor(root, new AdhocWorkspace());
                        foreach (var item in removeCache)
                        {
                            editor.RemoveNode(item);
                        }
                        compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                    }
#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[Semantic]", "语法树替换", 3);
#endif
                }

                return compilation;
            };
        }
    }
}
