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

    private static bool _isCompleted = false;
    public static void Init(Func<AssemblyName, bool>? excludeReferencesFunc = null)
    {
        if (excludeReferencesFunc == null)
        {
            excludeReferencesFunc = item => false;
        }
        if (!_isCompleted)
        {
            lock (_lock)
            {
                if (_isCompleted)
                {
                    return;
                }
#if DEBUG
                Stopwatch stopwatch = new();
                stopwatch.Start();
#endif

                NatashaDomain.RefreshDefaultAssemblies(excludeReferencesFunc);
                //var task = Task.Run(() => { });

                //Mark1 : 89ms
                //Mark2 : 14ms
                IEnumerable<string> paths = DependencyContext
                    .Default
                    .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

                var result = Parallel.ForEach(paths,
                                    asmPath =>
                                    {
                                        if (asmPath.Contains("System.Private.CoreLib"))
                                        {

                                        }
                                        var asmName = AssemblyName.GetAssemblyName(asmPath);
                                        if (!excludeReferencesFunc(asmName))
                                        {
                                            NatashaReferenceDomain.AddDefaultReferenceAndUsing(asmName, asmPath);
                                        }
                                    });

                //task.Wait();
                while (!result.IsCompleted)
                {
                    Task.Delay(50);
                }
                _isCompleted = true;
#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[Domain]", "域初始化", 1);
#endif

            }
        }


    }

}
