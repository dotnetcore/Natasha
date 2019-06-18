using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha
{
    /// <summary>
    /// 类名反解器
    /// </summary>
    public static class ClassNameReverser
    {


        public readonly static ConcurrentDictionary<Type, string> _type_mapping;
        static ClassNameReverser() => _type_mapping = new ConcurrentDictionary<Type, string>();




        /// <summary>
        /// 获取类名，检查缓存
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetName<T>()
        {
            return GetName(typeof(T));
        }
        public static string GetName(Type type)
        {
            if (type==null)
            {
                return "";
            }
            if (!_type_mapping.ContainsKey(type))
            {
                _type_mapping[type] = Reverser(type);
            }
            return _type_mapping[type];
        }




        /// <summary>
        /// 类名反解
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static string Reverser(Type type)
        {
            //后缀
            string Suffix = string.Empty;


            //数组判别
            while (type.HasElementType)
            {
                if (type.IsArray)
                {
                    Suffix = "[]";
                }
                type = type.GetElementType();
            }


            //泛型判别
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
                //特殊类型判别
                if (type == typeof(void))
                {
                    return "void";
                }
                return type.Name + Suffix;
            }
        }
    }
}
