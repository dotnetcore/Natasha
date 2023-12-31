
/* 项目“Natasha.CSharp.Template (netstandard2.0)”的未合并的更改
在此之前:
using Natasha.CSharp.Extension.Inner;
在此之后:
using Natasha;
using Natasha.CSharp;
using Natasha.CSharp.Extension.Inner;
*/
using Natasha.CSharp.Extension.Inner;
using System;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;

namespace Natasha.CSharp.Template.Reverser
{
    /// <summary>
    /// 修饰符反解
    /// </summary>
    public static class ModifierReverser
    {
        /// <summary>
        /// 根据修饰符枚举，获取修饰符
        /// </summary>
        /// <param name="enumModifier">修饰符枚举</param>
        /// <returns></returns>
        public static string GetModifier(ModifierFlags enumModifier)
        {
            switch (enumModifier)
            {
                case ModifierFlags.Static:
                    return "static ";
                case ModifierFlags.Virtual:
                    return "virtual ";
                case ModifierFlags.New:
                    return "new ";
                case ModifierFlags.Abstract:
                    return "abstract ";
                case ModifierFlags.Override:
                    return "override ";
                case ModifierFlags.Async:
                    return "async ";
                case ModifierFlags.Const:
                    return "const ";
                case ModifierFlags.Readonly:
                    return "readonly ";
                case ModifierFlags.Fixed:
                    return "fixed ";
                default:
                    return "";
            }
        }




        /// <summary>
        /// 获取属性能否被重写
        /// </summary>
        /// <param name="reflectMethodInfo">方法反射信息</param>
        /// <returns>修饰符</returns>
        public static string? GetCanOverrideModifier(MethodInfo reflectMethodInfo)
        {

            //如果没有被重写
            var modifiter = GetModifier(reflectMethodInfo);
            if (modifiter == "abstract " || modifiter.StartsWith("virtual ") || modifiter.StartsWith("override "))
            {
                return modifiter;
            }
            return null;

        }


        /// <summary>
        /// 获取属性能否被重写
        /// </summary>
        /// <param name="propertyInfo">属性反射信息</param>
        /// <returns>修饰符</returns>
        public static string? GetCanOverrideModifier(PropertyInfo propertyInfo)
        {

            return GetCanOverrideModifier(propertyInfo.GetMethodInfo());

        }


        /// <summary>
        /// 获取事件能否被重写
        /// </summary>
        /// <param name="eventInfo">事件反射信息</param>
        /// <returns>修饰符</returns>
        public static string? GetCanOverrideModifier(EventInfo eventInfo)
        {

            return GetCanOverrideModifier(eventInfo.GetMethodInfo());

        }


        /// <summary>
        /// 获取属性的修饰符
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetModifier(PropertyInfo propertyInfo)
        {
            return GetModifier(propertyInfo.GetMethodInfo());
        }

        /// <summary>
        /// 获取事件的修饰符
        /// </summary>
        /// <param name="eventInfo"></param>
        /// <returns></returns>
        public static string GetModifier(EventInfo eventInfo)
        {
            return GetModifier(eventInfo.GetMethodInfo());
        }


        /// <summary>
        /// 获取方法的修饰符
        /// </summary>
        /// <param name="reflectMethodInfo">方法反射信息</param>
        /// <returns></returns>
        public static string GetModifier(MethodInfo reflectMethodInfo)
        {

            string result = AsyncReverser.GetAsync(reflectMethodInfo);

            if (reflectMethodInfo.IsStatic)
            {

                return "static " + result;

            }
            else if (reflectMethodInfo.DeclaringType != null && !reflectMethodInfo.DeclaringType!.IsInterface)
            {

                //如果没有被重写
                if (reflectMethodInfo.Equals(reflectMethodInfo.GetBaseDefinition()))
                {

                    if (reflectMethodInfo.IsAbstract)
                    {
                        return "abstract " + result;
                    }
                    else if (!reflectMethodInfo.IsFinal && reflectMethodInfo.IsVirtual)
                    {
                        return "virtual " + result;
                    }
                    else
                    {

                        var baseType = reflectMethodInfo.DeclaringType.BaseType;
                        if (baseType != null && baseType != typeof(object))
                        {
                            var baseInfo = baseType
                            .GetMethod(reflectMethodInfo.Name, BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.NonPublic);
                            if (baseInfo != null && reflectMethodInfo != baseInfo)
                            {
                                return "new " + result;
                            }
                        }

                    }

                }
                else
                {
                    return "override " + result;
                }

            }
            return result;

        }



        /// <summary>
        /// 获取字段的修饰符
        /// </summary>
        /// <param name="reflectFieldInfo">字段反射信息</param>
        /// <returns></returns>
        public static string GetModifier(FieldInfo reflectFieldInfo)
        {

            if (reflectFieldInfo.IsLiteral)
            {

                return "const ";

            }


            var result = "";
            if (reflectFieldInfo.IsStatic)
            {

                result = "static ";

            }


            if (reflectFieldInfo.IsInitOnly)
            {

                result += "readonly ";

            }

            if (reflectFieldInfo.DeclaringType != null)
            {
                var baseType = reflectFieldInfo.DeclaringType.BaseType;
                if (baseType != null && baseType != typeof(object))
                {
                    var baseInfo = baseType
                    .GetMethod(reflectFieldInfo.Name, BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.NonPublic);
                    if (reflectFieldInfo != baseInfo)
                    {
                        return "new ";
                    }
                }
            }

            return result;
        }




        /// <summary>
        /// 获取类型的修饰符信息
        /// </summary>
        /// <param name="info">类型</param>
        /// <returns></returns>
        public static string GetModifier(Type info)
        {

            StringBuilder builder = new StringBuilder();
            if (info.IsClass)
            {

                if (info.IsAbstract)
                {

                    if (info.IsSealed)
                    {
                        builder.Append("static ");
                    }
                    else
                    {
                        builder.Append("abstract ");
                    }

                }
                else if (info.IsSealed)
                {
                    builder.Append("sealed ");
                }

            }

#if NETCOREAPP2_1_OR_GREATER
            else if (info.IsValueType)
            {

                if (!info.IsEnum)
                {

                    if (info.GetCustomAttribute<IsReadOnlyAttribute>() != default)
                    {
                        builder.Append("readonly ");
                    }
                    if (info.IsByRefLike)
                    {
                        builder.Append("ref ");
                    }

                }

            }
#endif


            return builder.ToString();
        }

    }

}
