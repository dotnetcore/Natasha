using System;
using System.Collections.Concurrent;

public sealed class DomainManagement
{

    public static readonly ConcurrentDictionary<string, WeakReference> Cache;

    static DomainManagement()
    {
        Cache = new ConcurrentDictionary<string, WeakReference>();
    }


    public static NatashaLoadContext Random()
    {
        return Create("N" + Guid.NewGuid().ToString("N")); 
    }


    public static NatashaLoadContext Create(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (NatashaLoadContext)(Cache[key].Target!);
        }
        else
        {
            Clear();
            var domain = new NatashaLoadContext(key);
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



    public static void Add(string key, NatashaLoadContext domain)
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


    public static WeakReference Remove(string key)
    {
        if (Cache.ContainsKey(key))
        {
            var result = Cache!.Remove(key);
            if (result != default)
            {
                ((NatashaLoadContext)(result.Target!)).Dispose();
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


    public static NatashaLoadContext? Get(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (NatashaLoadContext)Cache[key].Target!;
        }
        return null;
    }

}