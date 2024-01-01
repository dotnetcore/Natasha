using Natasha.CSharp.Extension.Inner;
using System;
using System.Reflection;

namespace Natasha.CSharp.Template.Reverser
{
    /// <summary>
    /// 访问级别反解器
    /// </summary>
    public static class AccessReverser
    {

        /// <summary>
        /// 根据枚举获取访问级别
        /// </summary>
        /// <param name="enumAccess">访问级别枚举</param>
        /// <returns></returns>
        public static string GetAccess(AccessFlags enumAccess)
        {

            switch (enumAccess)
            {

                case AccessFlags.Public:
                    return "public ";


                case AccessFlags.Private:
                    return "private ";


                case AccessFlags.Protected:
                    return "protected ";


                case AccessFlags.Internal:
                    return "internal ";


                case AccessFlags.InternalAndProtected:
                    return "internal protected ";

                default:
                    return "internal ";

            }

        }


        /// <summary>
        /// 获取属性的访问级别
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetAccess(PropertyInfo propertyInfo)
        {
            return GetAccess(propertyInfo.GetMethodInfo());
        }

        /// <summary>
        /// 获取方法的访问级别
        /// </summary>
        /// <param name="methodInfo">反射出的方法成员</param>
        /// <returns></returns>
        public static string GetAccess(MethodInfo methodInfo)
        {

            if (methodInfo.IsPublic)
            {

                return "public ";

            }
            else if (methodInfo.IsPrivate)
            {

                return "private ";

            }
            else if (methodInfo.IsAssembly)
            {

                return "internal ";

            }
            else if (methodInfo.IsFamily)
            {

                return "protected ";

            }
            else if (methodInfo.IsFamilyOrAssembly)
            {

                return "internal protected ";

            }


            return "internal ";

        }


        /// <summary>
        /// 获取字段的访问级别
        /// </summary>
        /// <param name="eventInfo">反射出的事件成员</param>
        /// <returns></returns>
        public static string GetAccess(EventInfo eventInfo)
        {

            return GetAccess(eventInfo.GetMethodInfo());

        }




        /// <summary>
        /// 获取字段的访问级别
        /// </summary>
        /// <param name="fieldInfo">反射出的字段成员</param>
        /// <returns></returns>
        public static string GetAccess(FieldInfo fieldInfo)
        {

            if (fieldInfo.IsPublic)
            {

                return "public ";

            }
            else if (fieldInfo.IsPrivate)
            {

                return "private ";

            }
            else if (fieldInfo.IsFamily)
            {

                return "protected ";

            }
            else if (fieldInfo.IsAssembly)
            {

                return "internal ";

            }
            else if (fieldInfo.IsFamilyOrAssembly)
            {

                return "internal protected ";

            }


            return "internal ";

        }




        /// <summary>
        /// 获取类型的访问级别
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetAccess<T>()
        {

            return GetAccess(typeof(T));

        }
        public static string GetAccess(Type type)
        {

            if (type.IsPublic)
            {

                return "public ";

            }
            else if (type.IsNotPublic)
            {

                return "internal ";

            }

            return "internal ";

        }

    }

}
