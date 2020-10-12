using Natasha.CSharp.Reverser;
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
            return TypeNameReverser.GetDevelopName(type);
        }


        public static string GetRuntimeName(this Type type)
        {
            return TypeNameReverser.GetRuntimeName(type);
        }


        public static Type With(this Type type,params Type[] types)
        {
            return type.MakeGenericType(types);
        }

    }
}
