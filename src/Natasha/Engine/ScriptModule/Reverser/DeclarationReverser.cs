using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
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
            declaration.Append(methodInfo.ReturnType.GetDevelopName());
            declaration.Append(methodInfo.Name);
            declaration.Append(GetParameters(methodInfo));
            return declaration.ToString();
        }




        /// <summary>
        /// 根据参数反射信息反解出参数类型
        /// </summary>
        /// <param name="parameterInfo">参数反射信息</param>
        /// <returns></returns>
        public static string GetParametersType(ParameterInfo parameterInfo)
        {
            //前缀
            string Prefix = string.Empty;


            //反解参数类型
            string result = parameterInfo.ParameterType.GetDevelopName();


            //特殊类型反解
            if (parameterInfo.ParameterType.Name.EndsWith("&"))
            {
                if (parameterInfo.IsIn)
                {
                    Prefix = "in ";
                }
                else if (parameterInfo.IsOut)
                {
                    Prefix = "out ";
                }
                else
                {
                    Prefix = "ref ";
                }
                return $"{Prefix}{result}";
            }
            else
            {
                return $"{result}";
            }
        }




        /// <summary>
        /// 根据参数反射信息反解参数定义
        /// </summary>
        /// <param name="parameterInfo">参数反射信息</param>
        /// <returns></returns>
        public static string GetParametersDeclaration(ParameterInfo parameterInfo)
        {
            //前缀
            string Prefix = string.Empty;


            //反解类名
            string result = parameterInfo.ParameterType.GetDevelopName();


            //特殊处理
            if (parameterInfo.ParameterType.Name.EndsWith("&"))
            {
                if (parameterInfo.IsIn)
                {
                    Prefix = "in ";
                }
                else if (parameterInfo.IsOut)
                {
                    Prefix = "out ";
                }
                else
                {
                    Prefix = "ref ";
                }
                return $"{Prefix}{result} {parameterInfo.Name}";
            }
            else
            {
                return $"{result} {parameterInfo.Name}";
            }
        }




        /// <summary>
        /// 根据字段反射信息反解字段定义
        /// </summary>
        /// <param name="reflectFieldInfo">字段反射信息</param>
        /// <returns></returns>
        public static string GetFieldDeclaration(FieldInfo reflectFieldInfo)
        {
            return reflectFieldInfo.FieldType.GetDevelopName() + " "+reflectFieldInfo.Name;
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
            if (reflectMethodInfo == null)
            {
                throw new Exception("参数模板传参不能为空！");
            }
            var parameters = reflectMethodInfo.GetParameters();
            return GetParameters(parameters);
        }
    }
}
