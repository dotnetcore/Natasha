using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha
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
        public static string GetName<T>()
        {

            return GetName(typeof(T));

        }
        public static string GetName(Type type)
        {

            if (type == null)
            {

                return "";

            }
            return Reverser(type);

        }




        /// <summary>
        /// 类名反解
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static string Reverser(Type type)
        {
            string fatherString = default;
            //外部类处理
            if (type.DeclaringType!=null)
            {
                fatherString = Reverser(type.DeclaringType)+".";
            }


            //后缀
            StringBuilder Suffix = new StringBuilder();


            //数组判别
            while (type.HasElementType)
            {

                if (type.IsArray)
                {
                    var ctor = type.GetConstructors()[0];
                    int count = ctor.GetParameters().Length;
                    if (count == 1)
                    {
                        Suffix.Append("[]");
                    }
                    else
                    {
                        Suffix.Append("[");
                        for (int i = 0; i < count - 1; i++)
                        {
                            Suffix.Append(",");
                        }
                        Suffix.Append("]");
                    }

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
                return fatherString+result.ToString();

            }
            else
            {

                //特殊类型判别
                if (type == typeof(void))
                {

                    return "void";

                }
                return fatherString+type.Name + Suffix;

            }
        }

    }

}
