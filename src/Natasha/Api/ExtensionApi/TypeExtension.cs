using Natasha;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System
{
    public static class TypeExtension
    {



        public static ConcurrentDictionary<Type, AssemblyDomain> _type_cache;
        static TypeExtension()
        {
            _type_cache = new ConcurrentDictionary<Type, AssemblyDomain>();
        }




        public static bool DisposeDomain(this Type type)
        {
            if (_type_cache.ContainsKey(type))
            {
                while (!_type_cache.TryRemove(type, out var domain))
                {
                    domain.Dispose();
                    return true;
                }
            }
            return false;
        }




        internal static void AddInCache(this Type type, AssemblyDomain domain)
        {
            _type_cache[type] = domain;
        }
    }
}
