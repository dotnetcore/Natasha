#if MULTI
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static readonly object _lock = new();

    private static bool _isCompleted = false;
    public static void Preheating(Func<AssemblyName, string?, bool>? excludeReferencesFunc = null)
    {

        if (!_isCompleted)
        {
            lock (_lock)
            {
                if (_isCompleted)
                {
                    return;
                }

                _isCompleted = true;

                excludeReferencesFunc ??= (_, _) => false;
#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                DefaultUsing.SetDefaultUsingFilter(excludeReferencesFunc);
                NatashaDomain.SetDefaultAssemblyFilter(excludeReferencesFunc);

                //Mark : 3.0M (Memory:2023-02-27)
                IEnumerable<string>? paths = NatashaReferencePathsHelper.GetReferenceFiles(excludeReferencesFunc);

#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[Reference]", "过滤初始化引用", 1);
#endif
                if (paths!=null && paths.Any())
                {
                    //Mark : 32.5M (Memory:2023-02-27)
                    //Console.WriteLine("Run?");
                    //Console.ReadKey();
                    ResolverMetadata(paths);
                    //Console.WriteLine("over");
                    //Console.ReadKey();
#if DEBUG
                    stopwatch.RestartAndShowCategoreInfo("[  Domain  ]", "默认信息初始化", 1);
#endif

                    AssemblyCSharpBuilder cSharpBuilder = new();
                    cSharpBuilder.ConfigCompilerOption(item => item.AddSupperess("CS8019").UseSuppressReportor(false));
                    using (DomainManagement.Random().CreateScope())
                    {
                        cSharpBuilder.EnableSemanticHandler = true;
                        cSharpBuilder.Add(DefaultUsing.UsingScript + "public class A{}");
                        //Mark : 22.0M (Memory:2023-02-27)
                        var assembly = cSharpBuilder.GetAssembly();
                    }
                    cSharpBuilder.Domain.Dispose();

#if DEBUG
                    stopwatch.StopAndShowCategoreInfo("[FirstCompile]", "初始化编译", 1);
#endif
                }

            }
        }

    }
    private static void ResolverMetadata(IEnumerable<string> paths)
    {
        var resolver = new PathAssemblyResolver(paths);
        using var mlc = new MetadataLoadContext(resolver);
        var result = Parallel.ForEach(paths, (path) =>
        {

            Assembly assembly = mlc.LoadFromAssemblyPath(path);
            DefaultUsing.AddUsingWithoutCheck(assembly,false);
            NatashaDomain.AddAssemblyToDefaultCache(assembly);
            NatashaReferenceDomain.DefaultDomain.References.AddReference(assembly.GetName(), path);

        });
        while (!result.IsCompleted)
        {
            Thread.Sleep(100);
        }
        DefaultUsing.ReBuildUsingScript();
    }

}
#endif