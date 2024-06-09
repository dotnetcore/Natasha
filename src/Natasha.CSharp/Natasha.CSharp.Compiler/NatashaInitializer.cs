using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Natasha.CSharp.Compiler.Component;

internal static class NatashaInitializer
{
    private static readonly object _lock = new();

    private static bool _isCompleted = false;
    private static Func<AssemblyName?, string?, bool>? _excludeReferencesFunc;

    public static void Preheating(Func<AssemblyName?, string?, bool>? excludeReferencesFunc = null
        , bool? useRuntimeUsing = false
        , bool? useRuntimeReference = false
        , bool useFileCache = false
        )
    {
        if (!_isCompleted)
        {
            lock (_lock)
            {
                if (_isCompleted)
                {
                    return;
                }

                _excludeReferencesFunc = excludeReferencesFunc;
#if DEBUG
                Stopwatch stopwatchTotal = new();
                stopwatchTotal.Start();
#endif

                AssemblyCSharpBuilder.HasInitialized = true;
                _isCompleted = true;

                var task = Task.Run(() =>
                {

                    AssemblyCSharpBuilder tempBuilder = new();
                    tempBuilder
                    .UseRandomLoadContext()
                    .UseSimpleMode()
                    .ConfigCompilerOption(opt => opt
                            .WithCompilerFlag(
                            Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressConstraintChecks |
                            Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressObsoleteChecks |
                            Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressTypeArgumentBinding |
                            Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressUnsafeDiagnostics |
                            Natasha.CSharp.Compiler.CompilerBinderFlags.GenericConstraintsClause |
                            Natasha.CSharp.Compiler.CompilerBinderFlags.UncheckedRegion)
                    )
                    .ConfigLoadContext(ldc => ldc.AddReferenceAndUsingCode(typeof(object)))
                    .FastAddScriptWithoutCheck("public class A{  public object? Test(){ return null; } }");
                    tempBuilder.CompileWithoutAssembly();
                    tempBuilder.Domain.Dispose();

                });

#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                Queue<ParallelLoopResult> parallelLoopResults = [];
                var namespaceCacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Natasha.Namespace.cache");
                //不需要处理 using
                if (File.Exists(namespaceCacheFilePath) && useFileCache)
                {
                    //从缓存文件中读取 using
                    var namespaceCache = File.ReadAllText(namespaceCacheFilePath, Encoding.UTF8);
                    var namespaceText = namespaceCache.Split(["+?"], StringSplitOptions.RemoveEmptyEntries);
                    NatashaLoadContext.DefaultContext.UsingRecorder.Using(namespaceText);
                    if (useRuntimeReference!=null)
                    {
                        if (useRuntimeReference.Value)
                        {
                            var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                            parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                        }
                        else
                        {
                            var paths = NatashaAssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
                            if (paths != null && paths.Any())
                            {
                                parallelLoopResults.Enqueue(InitReferenceFromPath(paths));
                            }
                            else
                            {
                                throw new Exception("Natasha 初始化失败，无法获取到引用程序集！建议引入：DotNetCore.Compile.Environment 环境包。");
                            }
                        }
                    }
                    
                }
                else
                {
                    if (useRuntimeReference != null)
                    {
                        if (useRuntimeReference.Value)
                        {
                            if (useRuntimeUsing!=null)
                            {
                                if (useRuntimeUsing.Value)
                                {
                                    var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                                    parallelLoopResults.Enqueue(InitReferenceAndUsingFromRuntime(assemblies));
                                }
                                else
                                {
                                    var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                                    parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                                    var paths = NatashaAssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
                                    if (paths != null && paths.Any())
                                    {
                                        parallelLoopResults.Enqueue(InitUsingFromPath(paths));
                                    }
                                    else
                                    {
                                        throw new Exception("Natasha 初始化失败，无法获取到引用程序集！建议引入：DotNetCore.Compile.Environment 环境包。");
                                    }
                                }
                            }
                            else
                            {
                                var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                                parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                            }
                        }
                        else
                        {
                            var paths = NatashaAssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
                            if (paths != null && paths.Any())
                            {
                                if (useRuntimeUsing!=null)
                                {
                                    if (useRuntimeUsing.Value)
                                    {
                                        parallelLoopResults.Enqueue(InitReferenceFromPath(paths));
                                        var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                                        parallelLoopResults.Enqueue(InitUsingFromRuntime(assemblies));
                                    }
                                    else
                                    {
                                        parallelLoopResults.Enqueue(InitReferenceAndUsingFromPath(paths));
                                    }
                                }
                                else
                                {
                                    parallelLoopResults.Enqueue(InitReferenceFromPath(paths));
                                }
                                
                            }
                            else
                            {
                                throw new Exception("Natasha 初始化失败，无法获取到引用程序集！建议引入：DotNetCore.Compile.Environment 环境包。");
                            }
                        }
                    }
                    else
                    {
                        if (useRuntimeUsing != null)
                        {
                            if (useRuntimeUsing.Value)
                            {
                                var assemblies = NatashaAssemblyHelper.GetRuntimeAssemblies();
                                parallelLoopResults.Enqueue(InitUsingFromRuntime(assemblies));
                            }
                            else
                            {
                                var paths = NatashaAssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
                                if (paths != null && paths.Any())
                                {
                                    parallelLoopResults.Enqueue(InitUsingFromPath(paths));
                                }
                                else
                                {
                                    throw new Exception("Natasha 初始化失败，无法获取到引用程序集！建议引入：DotNetCore.Compile.Environment 环境包。");
                                }
                            }
                        }
                    }
                }

                while (parallelLoopResults.Count > 0)
                {
                    var result = parallelLoopResults.Dequeue();
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(80);
                    }
                }
                NatashaLoadContext.DefaultContext.UsingRecorder.ToString();
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[  Metadata  ]", "编译缓存初始化", 1);
#endif
                if (useRuntimeReference!=null)
                {
                    AssemblyCSharpBuilder cSharpBuilder = new();
                    cSharpBuilder
                        .UseRandomLoadContext()
                        .UseSmartMode()
                        .FastAddScriptWithoutCheck(@"using System; public class B{ public object? Test(){ return null; } }")
                        .CompileWithoutAssembly();

                    cSharpBuilder.Domain.Dispose();
                }
               

                if (!File.Exists(namespaceCacheFilePath) && useFileCache)
                {
                    var namespaceCache = new StringBuilder();
                    foreach (var preNamespace in NatashaLoadContext.DefaultContext.UsingRecorder)
                    {
                        namespaceCache.Append($"{preNamespace}+?");
                    }
                    File.WriteAllText(namespaceCacheFilePath, namespaceCache.ToString(), Encoding.UTF8);
                }

                while (!task.IsCompleted)
                {
                    Thread.Sleep(50);
                }
#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[ConcurrentCompile]", "编译初始化", 1);
                stopwatchTotal.StopAndShowCategoreInfo("[    Total    ]", "总占用", 1);
#endif

                GC.Collect();

                static ParallelLoopResult InitReferenceFromRuntime(Assembly[] assemblies)
                {
                    return Parallel.ForEach(assemblies, assembly =>
                    {
                        var result = MetadataHelper.GetMetadataFromMemory(assembly, _excludeReferencesFunc);
                        if (result != null)
                        {
                            NatashaLoadContext.DefaultContext.ReferenceRecorder.AddReference(result.Value.asmName, result.Value.metadata, AssemblyCompareInformation.None);
                        }
                    });
                }
                static ParallelLoopResult InitReferenceAndUsingFromRuntime(Assembly[] assemblies)
                {
                    return Parallel.ForEach(assemblies, assembly =>
                    {
                        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly, _excludeReferencesFunc);
                        if (result.HasValue)
                        {
                            NatashaLoadContext.DefaultContext.AddReferenceAndUsingCode(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
                        }
                    });
                }
                static ParallelLoopResult InitUsingFromRuntime(Assembly[] assemblies)
                {
                    return Parallel.ForEach(assemblies, assembly =>
                    {
                        var namespaces = MetadataHelper.GetNamespaceFromMemroy(assembly, _excludeReferencesFunc);
                        NatashaLoadContext.DefaultContext.UsingRecorder.Using(namespaces);
                    });
                }
                unsafe static ParallelLoopResult InitUsingFromPath(IEnumerable<string> paths)
                {
                    return Parallel.ForEach(paths, (path) =>
                    {
                        var result = MetadataHelper.GetNamespaceFromFile(path, _excludeReferencesFunc);
                        if (result != null)
                        {
                            NatashaLoadContext.DefaultContext.UsingRecorder.Using(result);
                        }
                    });
                }
                unsafe static ParallelLoopResult InitReferenceFromPath(IEnumerable<string> paths)
                {
                    return Parallel.ForEach(paths, (path) =>
                    {
                        var result = MetadataHelper.GetMetadataFromFile(path, _excludeReferencesFunc);
                        if (result != null)
                        {
                            NatashaLoadContext.DefaultContext.ReferenceRecorder.AddReference(result.Value.asmName, result.Value.metadata, AssemblyCompareInformation.None);
                        }
                    });
                }
                unsafe static ParallelLoopResult InitReferenceAndUsingFromPath(IEnumerable<string> paths)
                {
                    return Parallel.ForEach(paths, (path) =>
                    {
                        var result = MetadataHelper.GetMetadataAndNamespaceFromFile(path, _excludeReferencesFunc);
                        if (result.HasValue)
                        {
                            NatashaLoadContext.DefaultContext.AddReferenceAndUsingCode(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
                        }
                    });
                }
            }
        }

    }

#if DEBUG
    private static readonly object _showLock = new();
    public static void Show(Assembly assembly)
    {
        lock (_showLock)
        {
            Console.WriteLine("Asssembly : " + assembly.FullName);
            Console.WriteLine("Attribute : " + string.Join(",", assembly.CustomAttributes.Select(item => item.AttributeType.Name)));
            Console.WriteLine("ReferenceLength : " + assembly?.GetReferencedAssemblies().Length);
            Console.WriteLine("ReferenceAsssembly : " + string.Join(",", assembly?.GetReferencedAssemblies().Select(asm => asm.Name)));
        }
    }
#endif
}