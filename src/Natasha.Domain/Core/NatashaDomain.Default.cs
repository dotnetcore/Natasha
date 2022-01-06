using Natasha.CSharp.Extension.Inner;
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
    private static Func<AssemblyName, bool> _excludeDefaultAssembliesFunc;

    static NatashaDomain()
    {
        _defaultAssembliesSets = new();
        _defaultAssemblyNameCache = new();
        _excludeDefaultAssembliesFunc = item => false;
    }

    public void SetExcludeDefaultAssemblyFunc(Func<AssemblyName, bool> func)
    {
        _excludeDefaultAssembliesFunc = func;
    }

    private static int _lockCount = 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetLock()
    {
        return Interlocked.CompareExchange(ref _lockCount, 1, 0) == 0;

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetAndWaitLock()
    {
        while (Interlocked.CompareExchange(ref _lockCount, 1, 0) != 0)
        {
            Thread.Sleep(20);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseLock()
    {

        _lockCount = 0;

    }


    private static void CheckAndIncrmentAssemblies()
    {
        if (GetLock())
        {
            var assemblies = Default.Assemblies;
            if (assemblies.Count() != _excludeCount + _defaultAssemblyNameCache.Count)
            {
                HashSet<Assembly> checkAsm = new HashSet<Assembly>(Default.Assemblies);
                checkAsm.ExceptWith(_defaultAssembliesSets);
                foreach (var item in checkAsm)
                {
                    var asmName = item.GetName();
                    if (_excludeDefaultAssembliesFunc(asmName))
                    {
                        _excludeCount += 1;
                    }
                    else
                    {
                        _defaultAssemblyNameCache[asmName.GetUniqueName()] = asmName;
                        if (!item.IsDynamic && !string.IsNullOrEmpty(item.Location))
                        {
                            DefaultDomainIncrementAssembly?.Invoke(item, item.Location);
                        }
                        _defaultAssembliesSets.Add(item);
                    }
                }
                _excludeCount += checkAsm.Count;
            }
            ReleaseLock();
        }

    }

    internal static int _excludeCount;
    public static void RefreshDefaultAssemblies(Func<AssemblyName, bool> excludeAssemblyNameFunc)
    {
        _excludeDefaultAssembliesFunc = excludeAssemblyNameFunc;
        CheckAndIncrmentAssemblies();
    }


}
