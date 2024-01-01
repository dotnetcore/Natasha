using System;
using System.Text;

namespace Natasha.CSharp.Reverser
{
    /// <summary>
    /// 类名反解器
    /// </summary>
    public static class TypeNameReverser
    {
        //private static readonly Func<string,bool> _nullableAttrCheck;

        //static TypeNameReverser()
        //{
        //    _nullableAttrCheck = (name) =>
        //    {
        //        return name.Contains("MaybeNullAttribute")
        //        || name.Contains("AllowNullAttribute")
        //        //|| name.Contains("NullableAttribute");
        //        || name.Contains("NullableContextAttribute");
        //    };

        //}

        /// <summary>
        /// 类名反解
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static string ReverseFullName(Type type, bool ignoreFlag = false)
        {

            string fatherString = string.Empty;
            //外部类处理
            if (type.DeclaringType != null && type.FullName != null)
            {
                fatherString = ReverseFullName(type.DeclaringType, ignoreFlag) + ".";
            }


            //后缀
            StringBuilder Suffix = new StringBuilder();


            //数组判别
            while (type!.HasElementType)
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
                type = type.GetElementType()!;

            }


            //泛型判别
            if (type.IsGenericType)
            {

                StringBuilder result = new StringBuilder();
                if (string.IsNullOrEmpty(fatherString) && !string.IsNullOrEmpty(type.Namespace) && !string.IsNullOrEmpty(type.FullName))
                {
                    result.Append(type.Namespace + ".");
                }
                result.Append($"{type.Name.Split('`')[0]}<");
                bool HasWriteArguments = false;


                if (type.GenericTypeArguments.Length > 0)
                {

                    HasWriteArguments = true;
                    result.Append(ReverseFullName(type.GenericTypeArguments[0], ignoreFlag));
                    for (int i = 1; i < type.GenericTypeArguments.Length; i++)
                    {

                        result.Append(',');
                        result.Append(ReverseFullName(type.GenericTypeArguments[i], ignoreFlag));

                    }

                }
                if (!HasWriteArguments)
                {

                    var types = ((System.Reflection.TypeInfo)type).GenericTypeParameters;
                    if (types.Length > 0)
                    {

                        if (!ignoreFlag)
                        {
                            result.Append(ReverseFullName(types[0], ignoreFlag));
                        }

                        for (int i = 1; i < types.Length; i++)
                        {

                            result.Append(',');
                            if (!ignoreFlag)
                            {
                                result.Append(ReverseFullName(types[i], ignoreFlag));
                            }

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
                if (string.IsNullOrEmpty(fatherString) && !string.IsNullOrEmpty(type.Namespace) && !string.IsNullOrEmpty(type.FullName))
                {
                    return type.Namespace + "." + type.Name + Suffix;
                }
                return fatherString + type.Name + Suffix;

            }
        }


        /// <summary>
        /// 类名反解
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string ReverseTypeName(Type type)
        {

            string fatherString = string.Empty;
            //外部类处理
            if (type.DeclaringType != null && type.FullName != null)
            {
                fatherString = ReverseTypeName(type.DeclaringType) + ".";
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
                type = type.GetElementType()!;

            }

            //if (type.CustomAttributes.Any(item => _nullableAttrCheck(item.AttributeType.Name)))
            //{
            //    Suffix.Append('?');
            //}

            //泛型判别
            if (type.IsGenericType)
            {

                StringBuilder result = new StringBuilder();
                result.Append($"{type.Name.Split('`')[0]}<");

                if (type.GenericTypeArguments.Length > 0)
                {

                    result.Append(ReverseTypeName(type.GenericTypeArguments[0]));
                    for (int i = 1; i < type.GenericTypeArguments.Length; i++)
                    {

                        result.Append(',');
                        result.Append(ReverseTypeName(type.GenericTypeArguments[i]));

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
                if (string.IsNullOrEmpty(fatherString) && !string.IsNullOrEmpty(type.Namespace) && !string.IsNullOrEmpty(type.FullName))
                {
                    return type.Name + Suffix;
                }
                return fatherString + type.Name + Suffix;

            }

        }
    }

}
