#if MULTI
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

                if (excludeReferencesFunc == null)
                {
                    excludeReferencesFunc = (_, _) => false;
                }
#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                DefaultUsing.SetDefaultUsingFilter(excludeReferencesFunc);
                NatashaDomain.SetDefaultAssemblyFilter(excludeReferencesFunc);


                IEnumerable<string>? paths = NatashaReferencePathsHelper.GetReferenceFiles(excludeReferencesFunc);

                //Parallel.ForEach(readonlyCompileLibraries, library =>
                //{
                //    var paths = library.ResolveReferencePaths()
                //          .Where(
                //             asmPath =>
                //             {
                //                 var asmName = AssemblyName.GetAssemblyName(asmPath);
                //                 return !excludeReferencesFunc(asmName, asmName.Name);
                //             });
                //    NatashaMetadataReader.ResolverMetadata(paths);
                //});
#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[Reference]", "过滤初始化引用", 1);
#endif
                if (paths!=null && paths.Count()>0)
                {
                    ResolverMetadata(paths);
#if DEBUG
                    stopwatch.RestartAndShowCategoreInfo("[  Domain  ]", "默认信息初始化", 1);
#endif

                    AssemblyCSharpBuilder cSharpBuilder = new();
                    cSharpBuilder.ConfigCompilerOption(item => item.AddSupperess("CS8019").UseSuppressReportor(false));
                    using (DomainManagement.Random().CreateScope())
                    {
                        cSharpBuilder.EnableSemanticHandler = true;
                        cSharpBuilder.Add(DefaultUsing.UsingScript + "public class A{}");
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
        using (var mlc = new MetadataLoadContext(resolver))
        {
            Parallel.ForEach(paths, (path) =>
            {

                Assembly assembly = mlc.LoadFromAssemblyPath(path);
                NatashaReferenceDomain.DefaultDomain.References.AddReference(assembly.GetName(), path);
                DefaultUsing.AddUsingWithoutCheck(assembly);
                NatashaDomain.AddAssemblyToDefaultCache(assembly);

            });
        }
    }

}
#endif