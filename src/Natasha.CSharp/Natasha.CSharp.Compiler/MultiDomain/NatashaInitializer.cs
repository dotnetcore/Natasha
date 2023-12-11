#if NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static readonly object _lock = new();

    private static bool _isCompleted = false;
    public static void Preheating(Func<AssemblyName, string?, bool>? excludeReferencesFunc = null
        , bool useRuntimeUsing = false
        , bool useRuntimeReference = false
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

#if DEBUG
                //StopwatchExtension.EnableMemoryMonitor();
                Stopwatch stopwatchTotal = new();
                stopwatchTotal.Start();
#endif
                AssemblyCSharpBuilder tempBuilder = new();
                tempBuilder
                .UseRandomDomain()
                .ConfigCompilerOption(opt => opt
                        .WithCompilerFlag(
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressConstraintChecks |
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressObsoleteChecks |
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressTypeArgumentBinding |
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressUnsafeDiagnostics |
                        Natasha.CSharp.Compiler.CompilerBinderFlags.GenericConstraintsClause |
                        Natasha.CSharp.Compiler.CompilerBinderFlags.UncheckedRegion)
                )
                .WithoutSemanticCheck()
                .WithoutCombineReferences()
                .WithCombineUsingCode(UsingLoadBehavior.None)
                .AddReference(typeof(object));
                var task = Task.Run(() =>
                {
                    tempBuilder.FastAddScriptWithoutCheck("public class A{}").GetAssembly();
                });

                AssemblyCSharpBuilder.HasInitialized = true;
                _isCompleted = true;

                excludeReferencesFunc ??= (_, _) => false;
#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                GlobalUsingHelper.SetDefaultUsingFilter(excludeReferencesFunc);
                NatashaDomain.SetDefaultAssemblyFilter(excludeReferencesFunc);
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[  Domain  ]", "默认信息初始化", 1);
#endif
                IEnumerable<string>? paths = null;
                Queue<ParallelLoopResult> parallelLoopResults = new();
               
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                parallelLoopResults.Enqueue(CacheRuntimeAssembly(assemblies));
                var namespaceCacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Natasha.Namespace.cache");

                if (useRuntimeReference)
                {
                    parallelLoopResults.Enqueue(InitReferenceFromRuntime(assemblies));
                }
                else
                {
                    paths = NatashaReferencePathsHelper.GetReferenceFiles(excludeReferencesFunc);
                    if (paths != null && paths.Any())
                    {
                        parallelLoopResults.Enqueue(InitReferenceFromPath(paths));
                    }
                }
                if (useRuntimeUsing)
                {
                    parallelLoopResults.Enqueue(InitUsingFromRuntime(assemblies));
                }
                else
                {
                    paths ??= NatashaReferencePathsHelper.GetReferenceFiles(excludeReferencesFunc);
                    if (paths != null && paths.Any())
                    {
                        //更新缓存
                        parallelLoopResults.Enqueue(InitUsingFromPath(paths));
                    }
                    else if(File.Exists(namespaceCacheFilePath))
                    {
                        //从缓存文件中读取
                        var namespaceCache = File.ReadAllText(namespaceCacheFilePath, Encoding.UTF8);
                        var namespaceText = namespaceCache.Split("+?", StringSplitOptions.RemoveEmptyEntries);
                        NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(namespaceText);
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
                if (!useRuntimeUsing && paths != null && paths.Any())
                {
                    var namespaceCache = new StringBuilder();
                    foreach (var preNamespace in NatashaReferenceDomain.DefaultDomain.UsingRecorder) 
                    {
                        namespaceCache.Append($"{preNamespace}+?");
                    }
                    File.WriteAllText(namespaceCacheFilePath, namespaceCache.ToString(), Encoding.UTF8);
                }
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[Reference]", "引用初始化", 1);
#endif
                AssemblyCSharpBuilder cSharpBuilder = new();
                cSharpBuilder.ConfigCompilerOption(item => item.AddSupperess("CS8019"));

                Console.WriteLine(cSharpBuilder.Domain.Name);
                cSharpBuilder
                    .UseRandomDomain()
                    .WithCombineUsingCode(UsingLoadBehavior.WithAll)
                    .FastAddScriptWithoutCheck(@"public class B{}")
                    .GetAssembly();

                tempBuilder.Domain.Dispose();
                cSharpBuilder.Domain.Dispose();
                while (!task.IsCompleted)
                {
                    Thread.Sleep(80);
                }
#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[ConcurrentCompile]", "编译初始化", 1);
                stopwatchTotal.StopAndShowCategoreInfo("[Total]", "总占用", 1);
#endif

                GC.Collect();
            }
        }

    }

#if DEBUG
    private static readonly object _showLock = new object();
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


    private static ParallelLoopResult InitReferenceFromRuntime(Assembly[] assemblies)
    {
        return Parallel.ForEach(assemblies, assembly =>
        {
            NatashaReferenceDomain.DefaultDomain.References.AddReference(assembly);
        });
    }
    //*
    private static ParallelLoopResult InitUsingFromRuntime(Assembly[] assemblies)
    {
        return Parallel.ForEach(assemblies, assembly =>
        {
            GlobalUsingHelper.AddUsingWithoutCheckingkAndInternalUsing(NatashaReferenceDomain.DefaultDomain, assembly);
        });
    }

    private static ParallelLoopResult CacheRuntimeAssembly(Assembly[] assemblies)
    {
        return Parallel.ForEach(assemblies, assembly =>
        {
            NatashaDomain.AddAssemblyToDefaultCache(assembly);
        });
    }
    ///*
    private unsafe static ParallelLoopResult InitUsingFromPath(IEnumerable<string> paths)
    {
        var resolver = new PathAssemblyResolver(paths);
        using var mlc = new MetadataLoadContext(resolver);
        return Parallel.ForEach(paths, (path) =>
        {

            Assembly assembly = mlc.LoadFromAssemblyPath(path);
            GlobalUsingHelper.AddUsingWithoutCheck(NatashaReferenceDomain.DefaultDomain, assembly);

        });
    }
    internal unsafe static ParallelLoopResult InitReferenceFromPath(IEnumerable<string> paths)
    {
        var resolver = new PathAssemblyResolver(paths);
        using var mlc = new MetadataLoadContext(resolver);
        return Parallel.ForEach(paths, (path) =>
        {
            Assembly assembly = mlc.LoadFromAssemblyPath(path);
            NatashaReferenceDomain.DefaultDomain.References.AddReference(assembly.GetName(), path);

        });
    }

}

#endif