using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Natasha.CSharp.Reverser
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
                default:
                    return "";
            }
        }



        /// <summary>
        /// 获取方法的修饰符
        /// </summary>
        /// <param name="reflectMethodInfo">方法反射信息</param>
        /// <returns></returns>
        public static string GetCanOverrideModifier(MethodInfo reflectMethodInfo)
        {

            //如果没有被重写
            if (reflectMethodInfo.Equals(reflectMethodInfo.GetBaseDefinition()))
            {

                string result = AsyncReverser.GetAsync(reflectMethodInfo);
                if (reflectMethodInfo.DeclaringType.IsInterface)
                {
                    return result;
                }
                else if (reflectMethodInfo.IsAbstract)
                {
                    return "abstract " + result;
                }
                else if (reflectMethodInfo.IsVirtual)
                {
                    return "virtual " + result;
                }

            }
            return null;

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

            }else if (!reflectMethodInfo.DeclaringType.IsInterface)
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
                        if (baseType != null && baseType != typeof(object) )
                        {
                            var baseInfo = reflectMethodInfo
                            .DeclaringType
                            .BaseType
                            .GetMethod(reflectMethodInfo.Name, BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.NonPublic);
                            if (reflectMethodInfo != baseInfo)
                            {
                                return "new " + result;
                            }
                        }
                        
                    }

                }
                else
                {
                    return result + "override ";
                }

            }
            return result+"";

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

#if !NETSTANDARD2_0
            else if (info.IsValueType)
            {

                if (!info.IsEnum)
                {

                    if (info.GetCustomAttribute<IsReadOnlyAttribute>()!=default)
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
