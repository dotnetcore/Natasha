using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class GenericTypeReverser
    {

        static GenericTypeReverser()
        {

        }

        public static List<Type> GetTypes(Type type)
        {
            List<Type> result = new List<Type>();
            result.Add(type);
            if (type.IsGenericType && type.FullName != null)
            {
                foreach (var item in type.GetGenericArguments())
                {
                    result.AddRange(GetTypes(item));
                }
            }
            return result;
        }
    }
}
