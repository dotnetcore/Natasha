using System;
using System.Collections.Generic;

namespace Natasha
{
    public static class TypeExtension
    {

        public static bool IsSimpleType(this Type type)
        {
            return type.IsValueType || type == typeof(string) || type == typeof(Delegate) || type == typeof(MulticastDelegate);
        }


        public static bool IsImplementFrom(this Type type,Type iType)
        {
            return new HashSet<Type>(type.GetInterfaces()).Contains(iType);
        }


        public static bool IsImplementFrom<T>(this Type type)
        {
            return new HashSet<Type>(type.GetInterfaces()).Contains(typeof(T));
        }


        public static List<Type> GetAllGenericTypes(this Type type)
        {
            List<Type> result = new List<Type>
            {
                type
            };
            if (type.IsGenericType && type.FullName != null)
            {
                foreach (var item in type.GetGenericArguments())
                {
                    result.AddRange(item.GetAllGenericTypes());
                }
            }
            return result;
        }


        public static List<Type> GetAllGenericTypes<T>()
        {
            return typeof(T).GetAllGenericTypes();
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
    }
}
