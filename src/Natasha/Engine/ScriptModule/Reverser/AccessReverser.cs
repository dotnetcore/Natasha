using System;
using System.Reflection;

namespace Natasha
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
        public static string GetAccess(AccessTypes enumAccess)
        {
            switch (enumAccess)
            {
                case AccessTypes.Public:
                    return "public ";
                case AccessTypes.Private:
                    return "private ";
                case AccessTypes.Protected:
                    return "protected ";
                case AccessTypes.Internal:
                    return "internal ";
                case AccessTypes.InternalAndProtected:
                    return "internal protected";
                default:
                    return "internal ";
            }
        }




        /// <summary>
        /// 获取方法的访问级别
        /// </summary>
        /// <param name="reflectMethodInfo">反射出的方法成员</param>
        /// <returns></returns>
        public static string GetAccess(MethodInfo reflectMethodInfo)
        {
            if (reflectMethodInfo.IsPublic)
            {
                return "public ";
            }
            else if (reflectMethodInfo.IsPrivate)
            {
                return "private ";
            }
            else if (reflectMethodInfo.IsFamily)
            {
                return "protected ";
            }
            else if (reflectMethodInfo.IsAssembly)
            {
                return "internal ";
            }
            else if (reflectMethodInfo.IsFamilyOrAssembly)
            {
                return "internal protected";
            }
            return "internal ";
        }




        /// <summary>
        /// 获取字段的访问级别
        /// </summary>
        /// <param name="reflectFieldInfo">反射出的字段成员</param>
        /// <returns></returns>
        public static string GetAccess(FieldInfo reflectFieldInfo)
        {
            if (reflectFieldInfo.IsPublic)
            {
                return "public ";
            }
            else if (reflectFieldInfo.IsPrivate)
            {
                return "private ";
            }
            else if (reflectFieldInfo.IsFamily)
            {
                return "protected ";
            }
            else if (reflectFieldInfo.IsAssembly)
            {
                return "internal ";
            }
            else if (reflectFieldInfo.IsFamilyOrAssembly)
            {
                return "internal protected";
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
            else if (type.IsNestedFamily)
            {
                return "protected ";
            }
            else if (type.IsNestedAssembly)
            {
                return "internal protected ";
            }
            return "internal ";
        }
    }
}
