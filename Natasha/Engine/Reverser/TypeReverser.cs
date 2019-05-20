using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.Engine.Builder.Reverser
{

    public static class TypeReverser
    {
        public static ConcurrentDictionary<Type, string> _type_mapping;
        static TypeReverser()
        {
            _type_mapping = new ConcurrentDictionary<Type, string>();
        }

        public static string Get(Type type)
        {
            if (!_type_mapping.ContainsKey(type))
            {
                _type_mapping[type] = Reverser(type);
            }
            return _type_mapping[type];
        }
        public static string Reverser(Type type)
        {
            string Suffix = string.Empty;
            while (type.HasElementType)
            {
                if (type.IsArray)
                {
                    Suffix = "[]";
                }
                type = type.GetElementType();
            }
            if (type.IsGenericType)
            {
                StringBuilder result = new StringBuilder();
                result.Append($"{type.Name.Split('`')[0]}<");
                if (type.GenericTypeArguments.Length > 0)
                {
                    result.Append(Reverser(type.GenericTypeArguments[0]));
                    for (int i = 1; i < type.GenericTypeArguments.Length; i++)
                    {
                        result.Append(',');
                        result.Append(Reverser(type.GenericTypeArguments[i]));
                    }
                }
                result.Append('>');
                result.Append(Suffix);
                return result.ToString();
            }
            else
            {
                if (type==typeof(void))
                {
                    return "void";
                }
                return type.Name + Suffix;
            }
        }
    }
}
