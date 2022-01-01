using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;

public static class DomainComponent
{
    private static readonly object _lock = new object();
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
            //Mark1 : 89ms
            //Mark2 : 14ms
            IEnumerable<string> paths = DependencyContext
                .Default
                .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

            Parallel.ForEach(AssemblyLoadContext.Default.Assemblies,
                              asm =>{ NatashaDomain.DefaultDomain.ReferenceCache.AddReference(asm); });

            var result = Parallel.ForEach(paths,
                                asm =>
                                {
                                    var key = Path.GetFileNameWithoutExtension(asm);
                                    if (!excludeReferencesFunc!(key))
                                    {
                                        DefaultUsing.AddReference(asm);
                                    }

                                });


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
