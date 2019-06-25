using System;
using System.Collections.Generic;

namespace Natasha
{
    public static class TypeExtension
    {
        public static bool IsImplementFrom(this Type type,Type iType)
        {
            HashSet<Type> types = new HashSet<Type>(type.GetInterfaces());
            return types.Contains(iType);
        }
        public static bool IsImplementFrom<T>(this Type type)
        {
            HashSet<Type> types = new HashSet<Type>(type.GetInterfaces());
            return types.Contains(typeof(T));
        }

        public static List<Type> GetAllGenericTypes(this Type type)
        {
            List<Type> result = new List<Type>();
            result.Add(type);
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
            return ClassNameReverser.GetName(type);
        }

        public static Type With(this Type type,params Type[] types)
        {
            return type.MakeGenericType(types);
        }


        public static bool IsOnceType(this Type type)
        {
            if (type==null){return false;}
            return type.IsPrimitive
                           || type == typeof(string)
                           || type == typeof(Delegate)
                           || type.IsEnum
                           || type == typeof(object)
                           || (!type.IsClass && !type.IsInterface);
        }
    }
}
