using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.CSharp.Component.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public static class NatashaInitializer
{
    private static readonly object _lock = new();

    private static bool _isCompleted = false;
    public static void Init(Func<AssemblyName, string?, bool>? excludeReferencesFunc = null)
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

                IEnumerable<string> paths = DependencyContext
                    .Default
                    .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths().Where(asmPath => {

                        var asmName = AssemblyName.GetAssemblyName(asmPath);
                        return !excludeReferencesFunc(asmName, asmName.Name);

                    }));


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
                NatashaMetadataReader.ResolverMetadata(paths);

#if DEBUG
                stopwatch.RestartAndShowCategoreInfo("[Domain]", "初始化默认域", 1);
#endif

                AssemblyCSharpBuilder cSharpBuilder = new AssemblyCSharpBuilder();
                cSharpBuilder.ConfigCompilerOption(item => item.AddSupperess("CS8019"));
                using (DomainManagement.Random().CreateScope())
                {
                    cSharpBuilder.EnableSemanticHandler = true;
                    cSharpBuilder.Add(DefaultUsing.UsingScript+"public class A{}");
                    var assembly = cSharpBuilder.GetAssembly();
                }
                cSharpBuilder.Domain.Dispose();

#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[Comile]", "初始化编译", 1);
#endif
            }
        }


    }

}
