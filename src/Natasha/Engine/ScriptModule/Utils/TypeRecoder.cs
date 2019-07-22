using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha.Utils
{
    public class TypeRecoder
    {
        public HashSet<Type> Types;
        public TypeRecoder()
        {
            Types = new HashSet<Type>();
        }

        public TypeRecoder Union(TypeRecoder recoder)
        {
            Types.UnionWith(recoder.Types);
            return this;
        }

        public TypeRecoder Add(params Type[] types)
        {
            Types.UnionWith(types);
            return this;
        }

        public TypeRecoder Add(IEnumerable<Type> types)
        {
            Types.UnionWith(types);
            return this;
        }

        public TypeRecoder Add(params MethodInfo[] infos)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                Types.UnionWith(From(infos[i]));
            }
            return this;
        }

        public static IEnumerable<Type> From(MethodInfo info)
        {
            List<Type> types = new List<Type>();
            types.Add(info.ReturnType);
            types.AddRange(info.GetParameters().Select(item => item.ParameterType));
            return types;
        }
        public static Type From(FieldInfo info)
        {
            return info.FieldType;
        }
        public static Type From(PropertyInfo info)
        {
            return info.PropertyType;
        }
        public static Type From(ParameterInfo info)
        {
            return info.ParameterType;
        }
    }
}
