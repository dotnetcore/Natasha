using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Engine.Template
{
    public class ParametersTemplate
    {
        public static StringBuilder GetParameters(params ParameterInfo[] parameters)
        {
            StringBuilder result = new StringBuilder();
            result.Append('(');
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    result.Append(DeclarationReverser.GetDeclaration(parameters[0]));
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        result.Append(',');
                        result.Append(DeclarationReverser.GetDeclaration(parameters[i]));
                    }
                }
            }
            result.Append(')');
            return result;
        }
        public static StringBuilder GetParameters(IEnumerable<KeyValuePair<Type, string>> list)
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
            return result;
        }
        public static StringBuilder GetParameters(MethodInfo info)
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
