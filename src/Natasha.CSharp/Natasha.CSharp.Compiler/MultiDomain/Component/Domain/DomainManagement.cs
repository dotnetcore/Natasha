#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Collections.Concurrent;
using static System.Runtime.Loader.AssemblyLoadContext;

public sealed class DomainManagement
{

    public static readonly ConcurrentDictionary<string, WeakReference> Cache;

    static DomainManagement()
    {
        Cache = new ConcurrentDictionary<string, WeakReference>();
    }


    public static NatashaReferenceDomain Random()
    {
        return Create("N" + Guid.NewGuid().ToString("N")); 
    }


    public static NatashaReferenceDomain Create(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (NatashaReferenceDomain)(Cache[key].Target!);
        }
        else
        {
            Clear();
            var domain = new NatashaReferenceDomain(key);
            Add(key, domain);
            return domain;
        }
    }


    public static void Clear()
    {
        foreach (var item in Cache)
        {
            if (!item.Value.IsAlive)
            {
                Cache!.Remove(item.Key);
            }
        }
    }



    public static void Add(string key, NatashaReferenceDomain domain)
    {
        if (Cache.ContainsKey(key))
        {
            if (!Cache[key].IsAlive)
            {
                Cache[key] = new WeakReference(domain);
            }
        }
        else
        {
            Cache[key] = new WeakReference(domain, trackResurrection: true);
        }
    }

    public static NatashaReferenceDomain CurrentDomain
    {
        get
        {
            return CurrentContextualReflectionContext == default ?
                NatashaReferenceDomain.DefaultDomain :
                (NatashaReferenceDomain)CurrentContextualReflectionContext;
        }
    }


    public static WeakReference Remove(string key)
    {
        if (Cache.ContainsKey(key))
        {
            var result = Cache!.Remove(key);
            if (result != default)
            {
                ((NatashaReferenceDomain)(result.Target!)).Dispose();
            }
            return result!;
        }

        throw new System.Exception($"Can't find key : {key}!");
    }


    public static bool IsDeleted(string key)
    {
        if (Cache.TryGetValue(key,out var value))
        {
            return !value!.IsAlive;
        }
        return true;
    }


    public static NatashaReferenceDomain? Get(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (NatashaReferenceDomain)Cache[key].Target!;
        }
        return null;
    }

}
#endif