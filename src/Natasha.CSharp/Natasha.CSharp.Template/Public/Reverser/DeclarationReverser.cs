using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template.Reverser
{
    /// <summary>
    /// 声明反解
    /// </summary>
    public static class DeclarationReverser
    {
        /// <summary>
        /// 根据元数据反解出函数信息
        /// </summary>
        /// <param name="methodInfo">方法元数据</param>
        /// <returns></returns>
        public static string GetMethodDeclaration(MethodInfo methodInfo)
        {

            StringBuilder declaration = new StringBuilder();
            declaration.Append(AccessReverser.GetAccess(methodInfo));
            declaration.Append(ModifierReverser.GetModifier(methodInfo));
            declaration.Append(GetReturnPrefix(methodInfo) + methodInfo.ReturnType.GetDevelopName());
            declaration.Append(methodInfo.Name);
            declaration.Append(GetParameters(methodInfo));
            return declaration.ToString();

        }


        /// <summary>
        /// 获取返回值的修饰符
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetReturnPrefix(MethodInfo info)
        {
            var parameterInfo = info.ReturnParameter;
            //特殊类型反解
            if (parameterInfo.ParameterType.Name.EndsWith("&"))
            {

                //前
                if (parameterInfo.IsIn)
                {

                    return "in ";

                }
                else if (parameterInfo.IsOut)
                {

                    return "out ";

                }
                else
                {

                    return "ref ";

                }

            }
            else
            {

                return string.Empty;

            }
        }

        public static string GetParametePrefix(ParameterInfo parameterInfo)
        {

            //特殊类型反解
            if (parameterInfo.ParameterType.Name.EndsWith("&"))
            {

                //前
                if (parameterInfo.IsIn)
                {

                    return "in ";

                }
                else if (parameterInfo.IsOut)
                {

                    return "out ";

                }
                else
                {

                    return "ref ";

                }

            }
            else
            {

                return string.Empty;

            }

        }
        /// <summary>
        /// 根据参数反射信息反解出参数类型
        /// </summary>
        /// <param name="parameterInfo">参数反射信息</param>
        /// <returns></returns>
        public static string GetParametersType(ParameterInfo parameterInfo)
        {

            //反解参数类型
            string result = parameterInfo.ParameterType.GetDevelopName();
            return $"{GetParametePrefix(parameterInfo)}{result}";

        }




        /// <summary>
        /// 根据参数反射信息反解参数定义
        /// </summary>
        /// <param name="parameterInfo">参数反射信息</param>
        /// <returns></returns>
        public static string GetParametersDeclaration(ParameterInfo parameterInfo)
        {

            //反解类名
            string result = parameterInfo.ParameterType.GetDevelopName();
            //特殊处理
            return $"{GetParametePrefix(parameterInfo)}{result} {parameterInfo.Name}";

        }




        /// <summary>
        /// 根据字段反射信息反解字段定义
        /// </summary>
        /// <param name="reflectFieldInfo">字段反射信息</param>
        /// <returns></returns>
        public static string GetFieldDeclaration(FieldInfo reflectFieldInfo)
        {

            return reflectFieldInfo.FieldType.GetDevelopName() + " " + reflectFieldInfo.Name;

        }




        /// <summary>
        /// 根据参数反射信息集合反解函数参数定义，带括号
        /// </summary>
        /// <param name="parametersInfo">参数集合</param>
        /// <returns></returns>
        public static string GetParameters(params ParameterInfo[] parametersInfo)
        {

            StringBuilder result = new StringBuilder();
            result.Append('(');
            if (parametersInfo != null)
            {

                if (parametersInfo.Length > 0)
                {

                    result.Append(GetParametersDeclaration(parametersInfo[0]));
                    for (int i = 1; i < parametersInfo.Length; i++)
                    {

                        result.Append(',');
                        result.Append(GetParametersDeclaration(parametersInfo[i]));

                    }

                }

            }
            result.Append(')');
            return result.ToString();

        }
        public static string GetParameters(IEnumerable<KeyValuePair<Type, string>> list)
        {

            StringBuilder result = new StringBuilder();
            result.Append('(');
            if (list != null)
            {

                int i = 0;
                foreach (var item in list)
                {

                    if (i > 0)
                    {

                        result.Append(',');

                    }

                    result.Append(item.Key.GetDevelopName());
                    result.Append(' ');
                    result.Append(item.Value);
                    i += 1;

                }

            }
            result.Append(')');
            return result.ToString();

        }




        /// <summary>
        /// 根据方法反射信息获取参数定义
        /// </summary>
        /// <param name="reflectMethodInfo">方法反射信息</param>
        /// <returns></returns>
        public static string GetParameters(MethodInfo reflectMethodInfo)
        {
            var parameters = reflectMethodInfo.GetParameters();
            return GetParameters(parameters);

        }

    }

}
