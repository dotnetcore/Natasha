#if NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Public.Component.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static readonly object _lock = new();

    private static bool _isCompleted = false;
    private static Func<AssemblyName?, string?, bool>? _excludeReferencesFunc;
    public static void Preheating(Func<AssemblyName?, string?, bool>? excludeReferencesFunc = null
        , bool useRuntimeUsing = false
        , bool useRuntimeReference = false
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
                //StopwatchExtension.EnableMemoryMonitor();
                Stopwatch stopwatchTotal = new();
                stopwatchTotal.Start();
#endif

                AssemblyCSharpBuilder.HasInitialized = true;
                _isCompleted = true;

               
                //var randomDomain = DomainManagement.Random();
                
                var task = Task.Run(() =>
                {

                    AssemblyCSharpBuilder tempBuilder = new();
                    tempBuilder
                    .UseRandomDomain()
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
                    .AddReferenceAndUsingCode(typeof(object))
                    .FastAddScriptWithoutCheck("public class A{}");
                    tempBuilder.GetAssembly();
                    tempBuilder.Domain.Dispose();

                });



#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                //GlobalUsingHelper.SetDefaultUsingFilter(excludeReferencesFunc);
                NatashaDomain.SetDefaultAssemblyFilter(excludeReferencesFunc);
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[  Domain  ]", "默认信息初始化", 1);
#endif

                var assemblies = NatashaAsssemblyHelper.GetRuntimeAssemblies();
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[  Assembly  ]", "程序集扫描与加载", 1);
#endif
                Queue<ParallelLoopResult> parallelLoopResults = [];
                parallelLoopResults.Enqueue(CacheRuntimeAssembly(assemblies));

                var namespaceCacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Natasha.Namespace.cache");
                //不需要处理 using
                if(File.Exists(namespaceCacheFilePath) && useFileCache)
                {
                    //从缓存文件中读取 using
                    var namespaceCache = File.ReadAllText(namespaceCacheFilePath, Encoding.UTF8);
                    var namespaceText = namespaceCache.Split("+?", StringSplitOptions.RemoveEmptyEntries);
                    NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(namespaceText);
                    if (useRuntimeReference)
                    {
                        parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                    }
                    else
                    {
                        var paths = NatashaAsssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
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
                else
                {
                    if (useRuntimeReference)
                    {
                        if (useRuntimeUsing)
                        {
                            parallelLoopResults.Enqueue(InitReferenceAndUsingFromRuntime(assemblies));
                        }
                        else
                        {
                            parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                            var paths = NatashaAsssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
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
                        var paths = NatashaAsssemblyHelper.GetReferenceAssmeblyFiles(excludeReferencesFunc);
                        if (paths != null && paths.Any())
                        {
                            if (useRuntimeUsing)
                            {
                                parallelLoopResults.Enqueue(InitReferenceFromPath(paths));
                                parallelLoopResults.Enqueue(InitUsingFromRuntime(assemblies));
                            }
                            else
                            {
                                parallelLoopResults.Enqueue(InitReferenceAndUsingFromPath(paths));
                            }
                        }
                        else
                        {
                            throw new Exception("Natasha 初始化失败，无法获取到引用程序集！建议引入：DotNetCore.Compile.Environment 环境包。");
                        }
                    }
                }

                while (parallelLoopResults.Count>0)
                {
                    var result = parallelLoopResults.Dequeue();
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(80);
                    }
                }
                NatashaReferenceDomain.DefaultDomain.UsingRecorder.ToString();
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[  Metadata  ]", "编译缓存初始化", 1);
#endif
                AssemblyCSharpBuilder cSharpBuilder = new();
                cSharpBuilder.ConfigCompilerOption(item => item.AddSupperess("CS8019"));
                cSharpBuilder
                    .UseRandomDomain()
                    .UseSmartMode()
                    .FastAddScriptWithoutCheck(@"public class B{}")
                    .GetAssembly();

                cSharpBuilder.Domain.Dispose();

                if (!File.Exists(namespaceCacheFilePath) && useFileCache)
                {
                    var namespaceCache = new StringBuilder();
                    foreach (var preNamespace in NatashaReferenceDomain.DefaultDomain.UsingRecorder)
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
                            NatashaReferenceDomain.DefaultDomain.References.AddReference(result.Value.asmName, result.Value.metadata, AssemblyCompareInfomation.None);
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
                            NatashaReferenceDomain.DefaultDomain.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
                        }
                    });
                }
                static ParallelLoopResult InitUsingFromRuntime(Assembly[] assemblies)
                {
                    return Parallel.ForEach(assemblies, assembly =>
                    {
                        var namespaces = MetadataHelper.GetNamespaceFromMemroy(assembly, _excludeReferencesFunc);
                        NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(namespaces);
                    });
                }
                static ParallelLoopResult CacheRuntimeAssembly(Assembly[] assemblies)
                {
                    return Parallel.ForEach(assemblies, NatashaDomain.AddAssemblyToDefaultCache);
                }
                unsafe static ParallelLoopResult InitUsingFromPath(IEnumerable<string> paths)
                {
                    return Parallel.ForEach(paths, (path) =>
                    {
                        var result = MetadataHelper.GetNamespaceFromFile(path, _excludeReferencesFunc);
                        if (result != null)
                        {
                            NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(result);
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
                            NatashaReferenceDomain.DefaultDomain.References.AddReference(result.Value.asmName, result.Value.metadata, AssemblyCompareInfomation.None);
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
                            NatashaReferenceDomain.DefaultDomain.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
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

#endif