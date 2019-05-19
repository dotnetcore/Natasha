using Natasha.Standard;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Operator
{
    public class MethodOperator<T> : MethodOperator
    {
        public MethodOperator() : base(typeof(T)) { }
    }
    public class MethodOperator
    {
        private Type _type;
        private string _current_key;
        public Action<Type> UsingAction;
        public MethodOperator(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// 静态加类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static MethodOperator Loader(Type type)
        {
            return new MethodOperator(type);
        }

        public MethodOperator this[string key]
        {
            get{
                var info = _type.GetMethod(key);
                if (info==null)
                {
                    throw new Exception($"{key}函数不是{_type.Name}的成员函数！");
                }
                _current_key = key;
                return this;
            }
        }

        public string SetMethod(string content)
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo method = _type.GetMethod(_current_key);
            var parameters = method .GetParameters();
            UsingAction?.Invoke(method.ReturnType);
            sb.Append($"public {GetRealType(method.ReturnType.Name)} {method.Name}");
            sb.Append("(");
            if (parameters.Length > 0)
            {
                UsingAction?.Invoke(parameters[0].ParameterType);
                sb.Append($"{GetRealType(parameters[0].ParameterType.Name)} {parameters[0].Name}");
                for (int i = 1; i < parameters.Length; i++)
                {
                    UsingAction?.Invoke(parameters[i].ParameterType);
                    sb.Append($",{GetRealType(parameters[i].ParameterType.Name)} {parameters[i].Name}");
                }
            }
            sb.Append($"){{{content}}}");
            return sb.ToString();
        }

        private string GetRealType(string type)
        {
            if (type == "Void")
            {
                return "void";
            }
            else if (type.EndsWith("&"))
            {
                return $"ref {type.Substring(0, type.Length - 1)}";
            }
            return type;
        }
    }
}
