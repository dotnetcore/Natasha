using System;
using System.Collections.Concurrent;

namespace Natasha
{

    public class AssemblyManagment
    {

        public static ConcurrentDictionary<string, WeakReference> Cache;
        static AssemblyManagment()
        {

            Cache = new ConcurrentDictionary<string, WeakReference>();

        }
        




        public static AssemblyDomain Create(string key)
        {

            return new AssemblyDomain(key);

        }




        public static void Add(string key, AssemblyDomain domain)
        {

            if (Cache.ContainsKey(key))
            {

                ((AssemblyDomain)(Cache[key].Target)).Dispose();
                if (!Cache[key].IsAlive)
                {
                    Cache[key] = new WeakReference(domain);
                }

            }
            else
            {

                Cache[key] = new WeakReference(domain, trackResurrection:true);

            }
            
        }




        public static WeakReference Remove(string key)
        {

            if (Cache.ContainsKey(key))
            {
                Cache.TryRemove(key,out var result);
                return result;

            }

            throw new Exception($"Can't find key : {key}!");

        }




        public static bool IsDelete(string key)
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

    }

}
