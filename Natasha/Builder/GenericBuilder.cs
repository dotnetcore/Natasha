using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class GenericBuilder
    {
        public static Type GetType(Type gType,params Type[] types)
        {
            return gType.MakeGenericType(types);
        }
        public static Type GetType<G>(params Type[] types)
        {
            return typeof(G).MakeGenericType(types);
        }
    }
}
