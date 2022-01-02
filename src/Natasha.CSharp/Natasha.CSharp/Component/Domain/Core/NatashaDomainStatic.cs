using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain : AssemblyLoadContext, IDisposable
{
    public readonly static NatashaDomain DefaultDomain;

    private readonly static ConcurrentDictionary<string, AssemblyName> _defaultAssembliesCache;

    //private readonly static Func<AssemblyDependencyResolver, Dictionary<string, string>> _getDictionary;

    static NatashaDomain()
    {
        //var methodInfo = typeof(AssemblyDependencyResolver).GetField("_assemblyPaths", BindingFlags.NonPublic | BindingFlags.Instance);
        //_getDictionary = item => (Dictionary<string, string>)(methodInfo!.GetValue(item)!);
        DefaultDomain = new NatashaDomain();
        DomainManagement.Add("Default", DefaultDomain);
        _defaultAssembliesCache = new ConcurrentDictionary<string, AssemblyName>();
    }


    public static void RefreshDefaultAssemblies(Func<string, bool> excludeReferencesFunc)
    {
        var assemblies = Default.Assemblies;
        foreach (var item in assemblies)
        {
            var name = item.GetName().GetUniqueName();
            if (!excludeReferencesFunc(name))
            {
                _defaultAssembliesCache[name!] = item.GetName();
                DefaultDomain._referenceCache.AddReference(item);
            }
        }
    }
}
