using Microsoft.CodeAnalysis;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain : AssemblyLoadContext, IDisposable
{
    public readonly static NatashaDomain DefaultDomain;

    private readonly static ConcurrentDictionary<string, AssemblyName> _defaultAssembliesCache;

    //private readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> _getDictionary;
    private static Func<string, bool> _excludeReferencesFunc;
    static NatashaDomain()
    {
        _excludeReferencesFunc = (item)=>false;
        //var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
        //_getDictionary = item => (Dictionary<string, string>)(methodInfo!.GetValue(item)!);
        DefaultDomain = new NatashaDomain();
        DomainManagement.Add("Default", DefaultDomain);
        _defaultAssembliesCache = new ConcurrentDictionary<string, AssemblyName>();
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
            if (assemblies.Count() != _excludeCount + _defaultAssembliesCache.Count)
            {
                _excludeCount = 0;
                foreach (var item in assemblies)
                {
                    var name = item.GetName().GetUniqueName();
                    if (!_excludeReferencesFunc(name))
                    {
                        if (!_defaultAssembliesCache.ContainsKey(name))
                        {
                            _defaultAssembliesCache[name] = item.GetName();
                            DefaultDomain._referenceCache.AddReference(item);
                        }
                    }
                    else
                    {
                        _excludeCount += 1;
                    }
                }
            }
            ReleaseLock();
        }
        
    }

    internal static int _excludeCount;
    public static void RefreshDefaultAssemblies(Func<string, bool> excludeReferencesFunc)
    {
        _excludeReferencesFunc = excludeReferencesFunc;
        var assemblies = Default.Assemblies;
        foreach (var item in assemblies)
        {
            var name = item.GetName().GetUniqueName();
            if (!excludeReferencesFunc(name))
            {
                _defaultAssembliesCache[name!] = item.GetName();
                DefaultDomain._referenceCache.AddReference(item);
            }
            else
            {
                _excludeCount += 1;
            }
        }
    }
}
