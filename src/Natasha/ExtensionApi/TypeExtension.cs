using Natasha;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System
{
    public static class TypeExtension
    {

        public static bool IsSimpleType(this Type type)
        {
            return type.IsValueType || 
                type == typeof(string) || 
                type.IsSubclassOf(typeof(Delegate)) || 
                type == typeof(Delegate) ||
                type.IsSubclassOf(typeof(MulticastDelegate)) ||
                type == typeof(MulticastDelegate) || 
                type == typeof(Type);
        }


        public static bool IsImplementFrom(this Type type,Type iType)
        {
            return new HashSet<Type>(type.GetInterfaces()).Contains(iType);
        }


        public static bool IsImplementFrom<T>(this Type type)
        {
            return new HashSet<Type>(type.GetInterfaces()).Contains(typeof(T));
        }


        public static HashSet<Type> GetAllTypes(this Type type)
        {
            HashSet<Type> result = new HashSet<Type> { type };


            var temp = type;
            while (temp.HasElementType)
            {
                temp = temp.GetElementType();
            }


            result.Add(temp);
            if (temp.IsGenericType && temp.FullName != null)
            {
                foreach (var item in temp.GetGenericArguments())
                {
                    result.UnionWith(item.GetAllTypes());
                }
            }
            return result;

        }


        public static HashSet<Type> GetAllTypes<T>()
        {
            return typeof(T).GetAllTypes();
        }


        public static string GetAvailableName(this Type type)
        {
            return AvailableNameReverser.GetName(type);
        }


        public static string GetDevelopName(this Type type)
        {
            return TypeNameReverser.GetName(type);
        }


        public static Type With(this Type type,params Type[] types)
        {
            return type.MakeGenericType(types);
        }




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
