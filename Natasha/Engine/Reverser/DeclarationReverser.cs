using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public static class DeclarationReverser
    {
        //public readonly static ConcurrentDictionary<ParameterInfo, string> _type_mapping;
        //static DeclarationReverser()
        //{
        //    _type_mapping = new ConcurrentDictionary<ParameterInfo, string>();
        //}

        public static string GetParametersType(ParameterInfo info)
        {
            string Prefix = string.Empty;
            string result = NameReverser.GetName(info.ParameterType);
            if (info.ParameterType.Name.EndsWith("&"))
            {
                if (info.IsIn)
                {
                    Prefix = "in ";
                }
                else if (info.IsOut)
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
        public static string GetParametersDeclaration(ParameterInfo info)
        {
            string Prefix = string.Empty;
            string result = NameReverser.GetName(info.ParameterType);
            if (info.ParameterType.Name.EndsWith("&"))
            {
                if (info.IsIn)
                {
                    Prefix = "in ";
                }
                else if (info.IsOut)
                {
                    Prefix = "out ";
                }
                else
                {
                    Prefix = "ref ";
                }
                return $"{Prefix}{result} {info.Name}";
            }
            else
            {
                return $"{result} {info.Name}";
            }
        }

        public static string GetFieldDeclaration(FieldInfo info)
        {
            return NameReverser.GetName(info.FieldType) + " "+info.Name;
        }


        public static string GetParameters(params ParameterInfo[] parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append('(');
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    result.Append(GetParametersDeclaration(parameters[0]));
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        result.Append(',');
                        result.Append(GetParametersDeclaration(parameters[i]));
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

                    result.Append(NameReverser.GetName(item.Key));
                    result.Append(' ');
                    result.Append(item.Value);
                    i += 1;
                }
            }
            result.Append(')');
            return result.ToString();
        }
        public static string GetParameters(MethodInfo info)
        {
            if (info == null)
            {
                throw new Exception("参数模板传参不能为空！");
            }
            var parameters = info.GetParameters();
            return GetParameters(parameters);
        }
    }
}
