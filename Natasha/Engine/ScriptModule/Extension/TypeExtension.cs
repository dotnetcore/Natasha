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
    }
}
