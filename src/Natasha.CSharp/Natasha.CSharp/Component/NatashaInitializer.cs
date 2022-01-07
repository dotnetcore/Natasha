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
                stopwatch.StopAndShowCategoreInfo("[Domain]", "默认引用初始化过滤", 1);
                stopwatch.Restart();
#endif
                NatashaMetadataReader.ResolverMetadata(paths);
                _isCompleted = true;
#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[Domain]", "默认域初始化", 1);
#endif

            }
        }


    }

}
