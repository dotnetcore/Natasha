using Natasha.Domain.Extension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;


public partial class NatashaDomain : AssemblyLoadContext
{
    private readonly static ConcurrentDictionary<string, AssemblyName> _defaultAssemblyNameCache;
    private readonly static HashSet<Assembly> _defaultAssembliesSets;
    private static Func<AssemblyName, string? , bool> _excludeDefaultAssembliesFunc;
    public static readonly NatashaDomain DefaultDomain;

    static NatashaDomain()
    {
        DefaultDomain = default!;
        _defaultAssembliesSets = new();
        _defaultAssemblyNameCache = new();
        _excludeDefaultAssembliesFunc = (_,_)=>false;
    }

    private static int _lockCount = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GetLock()
    {
        return Interlocked.CompareExchange(ref _lockCount, 1, 0) == 0;

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GetAndWaitLock()
    {
        while (Interlocked.CompareExchange(ref _lockCount, 1, 0) != 0)
        {
            Thread.Sleep(20);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReleaseLock()
    {

        _lockCount = 0;

    }


    public static void SetDefaultAssemblyFilter(Func<AssemblyName, string?, bool> excludeAssemblyNameFunc)
    {
        _excludeDefaultAssembliesFunc = excludeAssemblyNameFunc;
    }

    
    public static void AddAssemblyToDefaultCache(Assembly assembly)
    {

        var asmName = assembly.GetName();
        if (!_excludeDefaultAssembliesFunc(asmName, asmName.Name))
        {
            _defaultAssemblyNameCache[asmName.GetUniqueName()] = asmName;
            lock (_defaultAssembliesSets)
            {
                _defaultAssembliesSets.Add(assembly);
            }
        }
        else
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + asmName.FullName);
#endif
        }

    }



    private static void CheckAndIncrmentAssemblies()
    {
        if (GetLock())
        {
           
            var assemblies = DefaultDomain.Assemblies;
            var count = assemblies.Count();
            if (count != _preDefaultAssemblyCount)
            {
                _preDefaultAssemblyCount = count;
                HashSet<Assembly> checkAsm = new(DefaultDomain.Assemblies);
                lock (_defaultAssembliesSets)
                {
                    checkAsm.ExceptWith(_defaultAssembliesSets);
                    foreach (var item in checkAsm)
                    {
                        
                        var asmName = item.GetName();
                        if (_excludeDefaultAssembliesFunc(asmName, asmName.Name))
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("[排除程序集]:" + asmName.FullName);
#endif
                        }
                        else
                        {
                            _defaultAssemblyNameCache[asmName.GetUniqueName()] = asmName;
                            _defaultAssembliesSets.Add(item);
                        }
                    }
                }
            }
            ReleaseLock();
        }

    }

    private static int _preDefaultAssemblyCount;
    public static int DefaultAssemblyCacheCount { get{ return _preDefaultAssemblyCount;  } }
    public static void RefreshDefaultAssemblies(Func<AssemblyName, string?, bool>? excludeAssemblyNameFunc)
    {
        if (excludeAssemblyNameFunc!=null)
        {
            SetDefaultAssemblyFilter(excludeAssemblyNameFunc);
        }
        CheckAndIncrmentAssemblies();
    }


}
