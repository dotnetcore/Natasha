using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Natasha.Core
{

    public static class DomainCache
    {

        private static readonly ConcurrentDictionary<Assembly, AssemblyDomain> _cache;
        static DomainCache()
        {
            _cache = new ConcurrentDictionary<Assembly, AssemblyDomain>();
        }




        public static void Add(Assembly assembly, AssemblyDomain domain)
        {

            if (!_cache.ContainsKey(assembly))
            {
                _cache[assembly] = domain;
            }
            
        }




        public static void Add(Type type, AssemblyDomain domain)
        {

            if (type == default)
            {
                throw new NullReferenceException("Add: Type is null! This method can't be passed a null instance.");
            }
            Add(type.Assembly, domain);

        }




        public static void Add(Delegate @delegate, AssemblyDomain domain)
        {

            if (@delegate == default)
            {
                throw new NullReferenceException("Add: Delegate is null! This method can't be passed a null instance.");
            }

            Add(@delegate.Method.Module.Assembly, domain);

        }




        public static void RemoveReferences(Delegate @delegate)
        {

            if (@delegate == default)
            {
                throw new NullReferenceException("RemoveReferences: Delegate is null! This method can't be passed a null instance.");
            }

            Remove(@delegate.Method.Module.Assembly);

        }




        public static void RemoveReferences(Type type)
        {

            if (type == default)
            {
                throw new NullReferenceException("RemoveReferences: Type is null! This method can't be passed a null instance.");
            }
            Remove(type.Assembly);

        }




        public static void RemoveReferences(Assembly assembly)
        {
            if (_cache.ContainsKey(assembly))
            {
                Remove(assembly);
                _cache[assembly].RemoveAssembly(assembly);
            }
        }




        public static bool Remove(Assembly assembly)
        {
            if (_cache.ContainsKey(assembly))
            {
                while (!_cache.TryRemove(assembly, out _)) ;
                return true;
            }
            return false;
        }




        public static void DisposeDomain(Delegate @delegate)
        {

            if (@delegate == default)
            {
                throw new NullReferenceException("DisposeDomain: Delegate is null! This method can't be passed a null instance.");
            }

            DisposeDomain(@delegate.Method.Module.Assembly);

        }




        public static void DisposeDomain(Type type)
        {

            if (type == default)
            {
                throw new NullReferenceException("DisposeDomain: Type is null! This method can't be passed a null instance.");
            }

            DisposeDomain(type.Assembly);

        }




        public static void DisposeDomain(Assembly assembly)
        {

            if (_cache.ContainsKey(assembly))
            {

                _cache[assembly].Dispose();

            }

        }



        public static void Clear(AssemblyDomain domain)
        {

            if (domain!=default)
            {

                foreach (var item in domain.AssemblyMappings)
                {
                    if (_cache.ContainsKey(item.Key))
                    {
                        while (!_cache.TryRemove(item.Key, out _)) ;
                    }
                }

            }
            
        }

    }

}
