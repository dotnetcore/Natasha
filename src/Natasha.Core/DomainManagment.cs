using System;
using System.Collections.Concurrent;
using static System.Runtime.Loader.AssemblyLoadContext;

namespace Natasha
{

    public class DomainManagment
    {

        public readonly static AssemblyDomain Default;
        public static ConcurrentDictionary<string, WeakReference> Cache;

        static DomainManagment()
        {

            Cache = new ConcurrentDictionary<string, WeakReference>();
            Default = new AssemblyDomain("Default");

        }




        public static AssemblyDomain Random
        {
            get { return DomainManagment.Create("N" + Guid.NewGuid().ToString("N")); }
        }



        public static AssemblyDomain Create(string key)
        {

            if (Cache.ContainsKey(key))
            {
                return (AssemblyDomain)(Cache[key].Target);
            }
            else
            {

                foreach (var item in Cache)
                {

                    var domain = (AssemblyDomain)(item.Value.Target);
                    if (!item.Value.IsAlive)
                    {
                        Cache.TryRemove(item.Key, out _);
                    }

                }
                return new AssemblyDomain(key);
            }

        }




#if  !NETSTANDARD2_0
        public static ContextualReflectionScope Lock(string key)
        {

            if (Cache.ContainsKey(key))
            {
                return ((AssemblyDomain)(Cache[key].Target)).EnterContextualReflection();
            }
            return Default.EnterContextualReflection();

        }
        public static ContextualReflectionScope Lock(AssemblyDomain domain)
        {

            return domain.EnterContextualReflection();

        }
        public static ContextualReflectionScope CreateAndLock(string key)
        {

            return Lock(Create(key));

        }
        public static AssemblyDomain CurrentDomain
        {
            get 
            {
                return CurrentContextualReflectionContext==default?
                    Default:
                    (AssemblyDomain)CurrentContextualReflectionContext; 
            }
        }
#endif





        public static void Add(string key, AssemblyDomain domain)
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
                    ((AssemblyDomain)(result.Target)).Dispose();
                }
                return result;

            }

            throw new Exception($"Can't find key : {key}!");

        }




        public static bool IsDeleted(string key)
        {

            if (Cache.ContainsKey(key))
            {
                return !Cache[key].IsAlive;
            }
            return true;

        }




        public static AssemblyDomain Get(string key)
        {

            if (Cache.ContainsKey(key))
            {
                return (AssemblyDomain)Cache[key].Target;
            }
            return null;

        }




        public static int Count(string key)
        {
            return ((AssemblyDomain)(Cache[key].Target)).Count;
        }

    }

}
