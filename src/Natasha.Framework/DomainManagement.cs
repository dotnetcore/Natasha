using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using static System.Runtime.Loader.AssemblyLoadContext;


public class DomainManagement
{
    public static DomainBase Default;
    public static readonly ConcurrentDictionary<string, WeakReference> Cache;

    static DomainManagement()
    {
        Cache = new ConcurrentDictionary<string, WeakReference>();
    }

    public static T RegisterDefault<T>() where T : DomainBase, new()
    {

        if (Default != null)
        {
            if (typeof(T) != Default.GetType())
            {
                Default = (new T()).GetInstance("Default");
            }
        }
        else
        {
            Default = (new T()).GetInstance("Default");
        }
        return (T)Default;

    }

    internal static void RegisterDefault(Type type)
    {
        DomainBase domain = (DomainBase)type.Assembly.CreateInstance(type.FullName);
        if (Default == null || (Default != null && Default.GetType() != type))
        {
            if (domain != null)
            {
                Default = domain.GetInstance("Default");
            }
        }
    }

    public static DomainBase Random
    {
        get { return Default.GetInstance("N" + Guid.NewGuid().ToString("N")); }
    }

    public static DomainBase Create(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (DomainBase)(Cache[key].Target);
        }
        else
        {
            Clear();
            return Default.GetInstance(key);
        }
    }

    public static void Clear()
    {
        foreach (var item in Cache)
        {
            if (!item.Value.IsAlive)
            {
                Cache.TryRemove(item.Key, out _);
            }
        }
    }

#if !NETSTANDARD2_0
        public static ContextualReflectionScope Lock(string key)
        {
            if (Cache.ContainsKey(key))
            {
                return ((DomainBase)(Cache[key].Target)).EnterContextualReflection();
            }
            return Default.EnterContextualReflection();
        }
        public static ContextualReflectionScope Lock(DomainBase domain)
        {
            return domain.EnterContextualReflection();
        }
        public static ContextualReflectionScope CreateAndLock(string key)
        {
            return Lock(Create(key));
        }
        public static DomainBase CurrentDomain
        {
            get
            {
                return CurrentContextualReflectionContext==default?
                    (DomainBase)Default :
                    (DomainBase)CurrentContextualReflectionContext;
            }
        }
#endif

    public static void Add(string key, DomainBase domain)
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
            Cache.TryRemove(key, out var result);
            if (result != default)
            {
                ((DomainBase)(result.Target)).Dispose();
            }
            return result;
        }

        throw new System.Exception($"Can't find key : {key}!");
    }

    public static bool IsDeleted(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return !Cache[key].IsAlive;
        }
        return true;
    }

    public static DomainBase Get(string key)
    {
        if (Cache.ContainsKey(key))
        {
            return (DomainBase)Cache[key].Target;
        }
        return null;
    }

    public static int Count(string key)
    {
        return ((DomainBase)(Cache[key].Target)).Count;
    }
}
