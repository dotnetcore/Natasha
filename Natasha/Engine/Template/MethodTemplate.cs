using Natasha.Engine.Builder.Reverser;
using Natasha.Engine.Reverser;
using Natasha.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Engine.Template
{
    public class MethodTemplate : IMethodPackage
    {
        private string _method_name;
        private readonly TypeRecoder _recoder;
        public MethodTemplate()
        {
            _recoder = new TypeRecoder();
            _parameters_types = new List<Type>();
            _parameters_mappings = new List<KeyValuePair<Type, string>>();
            _method_name = "NatashaDynamicMethod";
        }
        public bool HashMethodName()
        {
            return _method_name != "NatashaDynamicMethod";
        }
        public MethodTemplate MethodName(string name)
        {
            _method_name = name;
            return this;
        }
        public MethodTemplate MethodName(MethodInfo info)
        {
            _method_name = info.Name;
            return this;
        }

        private string _access;
        public MethodTemplate Access(MethodInfo access)
        {
            _access = AccessReverser.GetAccess(access);
            return this;
        }
        public MethodTemplate Access(AccessTypes access)
        {
            _access = AccessReverser.GetAccess(access);
            return this;
        }
        public MethodTemplate Access(string access)
        {
            _access = access;
            return this;
        }


        private string _modifier;
        public MethodTemplate Modifier(MethodInfo modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return this;
        }
        public MethodTemplate Modifier(Modifiers modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return this;
        }
        public MethodTemplate Modifier(string modifier)
        {
            _modifier = modifier;
            return this;
        }

        private string _body;
        public MethodTemplate Body(string body)
        {
            _body = body;
            return this;
        }

        private string _parameters;
        public MethodTemplate Parameter(MethodInfo info)
        {
            _recoder.Add(info);
            _parameters = ParametersTemplate.GetParameters(info).ToString();
            return this;
        }
        public MethodTemplate Parameter(string parameters)
        {
            _parameters = parameters;
            return this;
        }
        public MethodTemplate Parameter(IEnumerable<KeyValuePair<Type, string>> parameters)
        {
            _recoder.Add(parameters.Select(item => item.Key));
            _parameters = ParametersTemplate.GetParameters(parameters).ToString(); ;
            return this;
        }


        private readonly List<KeyValuePair<Type, string>> _parameters_mappings;
        private readonly List<Type> _parameters_types;
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public MethodTemplate Param<T>(string key)
        {
            return Param(typeof(T), key);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public MethodTemplate Param(Type type, string key)
        {
            _parameters_types.Add(type);
            _recoder.Add(type);
            _parameters_mappings.Add(new KeyValuePair<Type, string>(type, key));
            return this;
        }



        private string _return_string;
        private Type _return_type;
        /// <summary>
        /// 设置返回类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns></returns>

        public MethodTemplate Return<T>()
        {
            return Return(typeof(T));
        }
        public MethodTemplate Return(MethodInfo info)
        {
            return Return(info.ReturnType);
        }
        public MethodTemplate Return(Type type = null)
        {
            if (type==null)
            {
                type = typeof(void);
            }
            _return_type = type;
            _recoder.Add(type);
            _return_string = NameReverser.GetName(type);
            return this;
        }



        private Type _delegate_type;
        /// <summary>
        /// 打包成脚本
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            _delegate_type = DelegateBuilder.GetDelegate(_parameters_types.ToArray(), _return_type);
            StringBuilder builder = new StringBuilder();
            if (_parameters == null)
            {
                Parameter(_parameters_mappings);
            }
            builder.Append($"{_access}{_modifier}{_return_string} {_method_name}{_parameters}{{{_body}}}");
            return (_method_name, _recoder.Types, builder.ToString(), _delegate_type);
        }
    }
}
