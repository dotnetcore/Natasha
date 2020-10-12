using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.CSharp.Reverser
{
    /// <summary>
    /// 类名反解器
    /// </summary>
    public static class TypeNameReverser
    {


        /// <summary>
        /// 获取类名，检查缓存
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetRuntimeName<T>()
        {

            return GetRuntimeName(typeof(T));

        }
        public static string GetRuntimeName(Type type)
        {

            if (type == null)
            {

                return default;

            }
            return Reverser(type, true);

        }




        public static string GetDevelopName<T>()
        {

            return GetDevelopName(typeof(T));

        }
        public static string GetDevelopName(Type type)
        {

            if (type == null)
            {

                return default;

            }
            return Reverser(type);

        }




        /// <summary>
        /// 类名反解
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static string Reverser(Type type, bool ignoreFlag = false)
        {

            string fatherString = default;
            //外部类处理
            if (type.DeclaringType != null && type.FullName != null)
            {
                fatherString = Reverser(type.DeclaringType) + ".";
            }


            //后缀
            StringBuilder Suffix = new StringBuilder();


            //数组判别
            while (type.HasElementType)
            {

                if (type.IsArray)
                {

                    int count = type.GetArrayRank();

                    Suffix.Append("[");
                    for (int i = 0; i < count - 1; i++)
                    {
                        Suffix.Append(",");
                    }
                    Suffix.Append("]");

                }
                type = type.GetElementType();

            }


            //泛型判别
            if (type.IsGenericType)
            {

                StringBuilder result = new StringBuilder();
                if (fatherString == default && type.Namespace!= default && type.FullName != default)
                {
                    result.Append(type.Namespace+".");
                }
                result.Append($"{type.Name.Split('`')[0]}<");
                bool HasWriteArguments = false;


                if (type.GenericTypeArguments.Length > 0)
                {

                    HasWriteArguments = true;
                    result.Append(Reverser(type.GenericTypeArguments[0]));
                    for (int i = 1; i < type.GenericTypeArguments.Length; i++)
                    {

                        result.Append(',');
                        result.Append(Reverser(type.GenericTypeArguments[i]));

                    }

                }
                if (!ignoreFlag && !HasWriteArguments)
                {

                    var types = ((System.Reflection.TypeInfo)type).GenericTypeParameters;
                    if (types.Length > 0)
                    {

                        result.Append(Reverser(types[0], ignoreFlag));
                        for (int i = 1; i < types.Length; i++)
                        {

                            result.Append(',');
                            result.Append(Reverser(types[i], ignoreFlag));

                        }

                    }

                }


                result.Append('>');
                result.Append(Suffix);
                return fatherString + result.ToString();

            }
            else
            {

                //特殊类型判别
                if (type == typeof(void))
                {

                    return "void";

                }
                if (fatherString == default && type.Namespace != default && type.FullName != default)
                {
                    return type.Namespace + "." + type.Name + Suffix;
                }
                return fatherString + type.Name + Suffix;

            }
        }

    }

}
