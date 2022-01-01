using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

public static class DomainComponent
{
    private static readonly object _lock = new();
    public static void Init(Func<string, bool>? excludeReferencesFunc = null)
    {
        if (excludeReferencesFunc == null)
        {
            excludeReferencesFunc = item => false;
        }
        lock (_lock)
        {
            if (DefaultUsing.InitCompleted)
            {
                return;
            }
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif
            var task = Task.Run(() => { NatashaDomain.RefreshDefaultAssemblies(excludeReferencesFunc); });

            //Mark1 : 89ms
            //Mark2 : 14ms
            IEnumerable<string> paths = DependencyContext
                .Default
                .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

            var result = Parallel.ForEach(paths,
                                asmPath =>
                                {
                                    var key = Path.GetFileNameWithoutExtension(asmPath);
                                    if (!excludeReferencesFunc(key))
                                    {
                                        var asmName = AssemblyName.GetAssemblyName(asmPath);
                                        NatashaDomain.DefaultDomain.ReferenceCache.AddReference(asmName, asmPath);
                                        DefaultUsing.AddUsing(asmName);
                                    }
                                });

            task.Wait();
            while (!result.IsCompleted)
            {
                Task.Delay(50);
            }

            DefaultUsing.UsingGenerator();
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Domain]", "域初始化", 1);
#endif

        }

    }

}
